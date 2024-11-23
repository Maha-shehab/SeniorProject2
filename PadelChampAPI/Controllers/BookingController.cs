using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PadelChampAPI.DTOs;
using PadelChampAPI.Interfaces;
using PadelChampAPI.Models;
using PadelChampAPI.Services;
using Stripe;


namespace PadelChampAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IGenericRepository<Booking> repository, PaymentService paymentService,
    IGenericRepository<ApplicationUser> userRepository,
    IGenericRepository<PadelPlayground> padelRepository) : ControllerBase
{
    private readonly string StripeWebhookSecret =
        "whsec_5a4562139bc209ceb23262f63416e357cebe6dc30c4fc4a221f7768d33d62932";
    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        Console.WriteLine("Hello from webhook");

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                StripeWebhookSecret
            );

            // Handle the 'checkout.session.completed' event
            if (stripeEvent.Type == "checkout.session.completed")
            {
                Console.WriteLine("Checkout session completed!");

                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                if (session != null)
                {
                    // Retrieve metadata from session
                    var UserId = session.Metadata["user_id"];
                    var BookingDate = DateTime.Parse(session.Metadata["booking_date"]);
                    var UserEmail = session.Metadata["user_email"];
                    var PadelPlaygroundId = int.Parse(session.Metadata["playground_id"]);
                    var Duration = int.Parse(session.Metadata["duration"]);
                    var StartTime = TimeSpan.Parse(session.Metadata["time"]);
                    var Price = int.Parse(session.Metadata["price"]);

                    // Recreate booking object
                    var booking = new Booking
                    {
                        UserId = UserId,  // Assuming UserId is a Guid
                        BookingDate = BookingDate,
                        PadelPlaygroundId = PadelPlaygroundId,
                        Duration = Duration,
                        StartTime = StartTime,
                        Price = Price,
                    };

                    // Save booking to the database
                    await repository.Create(booking);
                    await repository.SaveChangesAsync();
                    Console.WriteLine($"Booking created: {booking.UserId} for {booking.BookingDate}");
                }
            }
            // Handle the 'payment_intent.succeeded' event
            else if (stripeEvent.Type == "payment_intent.succeeded")
            {
                Console.WriteLine("Payment succeeded!");

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    // Retrieve metadata
                    var UserId = paymentIntent.Metadata["user_id"];
                    var BookingDate = DateTime.Parse(paymentIntent.Metadata["booking_date"]);
                    var UserEmail = paymentIntent.Metadata["user_email"];
                    var PadelPlaygroundId = int.Parse(paymentIntent.Metadata["playground_id"]);
                    var Duration = int.Parse(paymentIntent.Metadata["duration"]);
                    var StartTime = TimeSpan.Parse(paymentIntent.Metadata["time"]);
                    var Price = int.Parse(paymentIntent.Metadata["price"]);

                    // Recreate booking object
                    var booking = new Booking
                    {
                        UserId = UserId,  // Assuming UserId is a Guid
                        BookingDate = BookingDate,
                        PadelPlaygroundId = PadelPlaygroundId,
                        Duration = Duration,
                        StartTime = StartTime,
                        Price = Price,
                    };

                    // Save booking to the database
                    await repository.Create(booking);
                    await repository.SaveChangesAsync();
                    Console.WriteLine($"Booking created: {booking.UserId} for {booking.BookingDate}");
                }
            }

            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Stripe exception: {e.Message}");
            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General exception: {ex.Message}");
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
    {
        if (booking.StartTime < new TimeSpan(12, 0, 0) || booking.StartTime >= new TimeSpan(24, 0, 0))
        {
            return BadRequest("Booking time must be between 12 PM and 12 AM.");
        }

        if (!await IsSlotAvailable(booking))
        {
            return Conflict("Selected slot is not available.");
        }


        await repository.Create(booking);
        await repository.SaveChangesAsync();
        await paymentService.CreatePaymentAsync(new PaymentRequest()
        {
            Amount = booking.Price * 100,
            Currency = "SAR",
            Description = "Reservation for padel playground",
            Email = (await userRepository.FindOne(user => user.Id == booking.UserId)).Email,
            booking = await repository.FindOne(oneBooking => booking.BookingId == oneBooking.BookingId)
        });

        return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, booking);
    }
    [HttpPost("payment-session")]
    public async Task<IActionResult> PaymentSession([FromBody] Booking booking)
    {
        if (booking.StartTime < new TimeSpan(12, 0, 0) || booking.StartTime >= new TimeSpan(24, 0, 0))
        {
            return BadRequest("Booking time must be between 12 PM and 12 AM.");
        }

        if (!await IsSlotAvailable(booking))
        {
            return Conflict("Selected slot is not available.");
        }


        //await repository.Create(booking);
        //await repository.SaveChangesAsync();
        return Ok(await paymentService.CreateCheckoutSessionAsync(new PaymentRequest()
        {
            Amount = booking.Price * 100,
            Currency = "SAR",
            Description = $"Reservation for {(await padelRepository.GetOne(booking.PadelPlaygroundId)).PlaygroundName}",
            Email = (await userRepository.FindOne(user => user.Id == booking.UserId)).Email,
            booking = booking
        }));
    }
    private async Task<bool> IsSlotAvailable(Booking booking)
    {
        var endTime = booking.StartTime.Add(TimeSpan.FromMinutes(booking.Duration));

        var existingBookings = await repository.Find(b =>
            b.PadelPlaygroundId == booking.PadelPlaygroundId && // Check for the same playground
            b.BookingDate.Date == booking.BookingDate.Date); // Check for the same date

        // Check for overlapping bookings
        foreach (var existingBooking in existingBookings)
        {
            var existingEndTime = existingBooking.StartTime.Add(TimeSpan.FromMinutes(existingBooking.Duration));

            // Check for overlapping conditions
            if (existingBooking.StartTime < endTime && booking.StartTime < existingEndTime)
            {
                return false; // Slot is not available
            }
        }

        return true; // Slot is available
    }

    // Optional: Get Booking by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var booking = await repository.GetOne(id);

        if (booking == null)
        {
            return NotFound();
        }

        return booking;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AvailableBookingDTO>>> GetAvailableBookings([FromQuery] DateTime date)
    {
        // 1) Retrieve existing bookings for the given date
        var bookedSlots = (await repository.Find(booking => booking.BookingDate.Date == date.Date)).ToList();
        Console.WriteLine(bookedSlots);
        // 2) Define the available time range: from 12 PM to 12 AM
        var startTime = new TimeSpan(12, 0, 0); // 12:00 PM
        var endTime = new TimeSpan(24, 0, 0);  // 12:00 AM

        // 3) Create a list to store available bookings with valid durations
        var availableBookings = new List<AvailableBookingDTO>();

        // 4) Loop through each possible slot in hourly intervals (12 PM to 12 AM)
        for (var currentTime = startTime; currentTime < endTime; currentTime = currentTime.Add(TimeSpan.FromMinutes(60)))
        {
            var validDurations = new List<int>();

            // Check if the 60-minute slot is available
            if (IsTimeSlotAvailable(currentTime, 60, bookedSlots))
            {
                validDurations.Add(60);
            }

            // Check if the 120-minute slot is available
            if (IsTimeSlotAvailable(currentTime, 120, bookedSlots))
            {
                validDurations.Add(120);
            }

            // Add only if there are valid durations
            if (validDurations.Any())
            {
                availableBookings.Add(new AvailableBookingDTO
                {
                    StartTime = currentTime,
                    Durations = validDurations
                });
            }
        }
        availableBookings.ForEach(booking =>
        {
            if (booking.StartTime == new TimeSpan(23, 0, 0))
                booking.Durations.Remove(120);
        });
        // 5) Return the list of available bookings
        return Ok(availableBookings);
    }

    // Helper Method to Check If a Slot is Available
    private bool IsTimeSlotAvailable(TimeSpan startTime, int duration, List<Booking> bookedSlots)
    {
        var endTime = startTime.Add(TimeSpan.FromMinutes(duration));

        foreach (var booking in bookedSlots)
        {
            var bookingEndTime = booking.StartTime.Add(TimeSpan.FromMinutes(booking.Duration));

            // Check if the new slot overlaps with the existing booking
            if (startTime < bookingEndTime && endTime > booking.StartTime)
            {
                return false; // Slot is not available
            }
        }

        return true; // Slot is available
    }

    [HttpGet("payment")]
    public async Task<IActionResult> Payment()
    {

        return Ok(await paymentService.CreatePaymentAsync(new PaymentRequest()
        {
            Amount = 20000,
            Currency = "SAR",
            Description = "Hello world",
            Email = "lol@gmail.com",
            booking = await repository.FindOne(booking => booking.BookingId == 1014)
        }));
    }

    [HttpGet("user-booking/{userId}")]
    public async Task<IActionResult> GetUserBookings([FromRoute] string userId)
    => Ok(await repository.FindAndPopulateAsync(booking=> booking.UserId == userId, booking => booking.PadelPlayground));

    [HttpGet("success")]
    public IActionResult Success() => File("~/success.html", "text/html");
}
