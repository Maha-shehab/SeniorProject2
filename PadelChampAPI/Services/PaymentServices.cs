using PadelChampAPI.Models;
using Stripe;
using Stripe.Checkout;
namespace PadelChampAPI.Services;

public class PaymentService
{
    public async Task<string> CreatePaymentAsync(PaymentRequest paymentRequest)
    {
        var options = new ChargeCreateOptions
        {
            Amount = paymentRequest.Amount,
            Currency = paymentRequest.Currency,
            Description = paymentRequest.Description,
            ReceiptEmail = paymentRequest.Email,
            Source = "tok_visa",
            Metadata = new Dictionary<string, string>
        {
            { "user_id", paymentRequest.booking.UserId },
            { "booking_id", paymentRequest.booking.BookingId.ToString() },
            { "booking_date",paymentRequest.booking.BookingDate.ToShortTimeString() },
                {"user_email",paymentRequest.Email }
            // Add later
        }
        };

        var service = new ChargeService();
        Charge charge = await service.CreateAsync(options);

        if (charge.Status == "succeeded")
        {
            return charge.Id; // Return charge ID or any other info you need
        }

        throw new Exception("Payment failed");
    }
    public async Task<string> CreateCheckoutSessionAsync(PaymentRequest paymentRequest)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = paymentRequest.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = paymentRequest.Description,
                            Description= $"{paymentRequest.booking.Duration} minutes\n" +
                            $"starting at {paymentRequest.booking.StartTime}\n" +
                            $"on {paymentRequest.booking.BookingDate:dddd, dd MMM yyyy}",
                            
                            
                        },
                        UnitAmount = paymentRequest.Amount, // Amount in cents (or the smallest currency unit)
                        
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "http://10.0.2.2:5079/api/booking/success", // Replace with your success URL
            CancelUrl = "http://127.0.0.1:5079/cancel.html", // Replace with your cancel URL
            Metadata = new Dictionary<string, string>
            {
                { "user_id", paymentRequest.booking.UserId },
                { "booking_date", paymentRequest.booking.BookingDate.ToShortTimeString() },
                { "user_email", paymentRequest.Email },
                { "playground_id",paymentRequest.booking.PadelPlaygroundId.ToString()},
                { "duration",paymentRequest.booking.Duration.ToString()},
                { "time",paymentRequest.booking.StartTime.ToString()},
                {"price",paymentRequest.booking.Price.ToString()},
            }
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session.Url; // Return the URL for Stripe Checkout
    }
}
