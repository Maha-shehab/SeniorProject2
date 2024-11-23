using PadelChampMobile.Models;
using PadelChampMobile.Services;
using System.Text.Json;

namespace PadelChampMobile;

public partial class Reservation : ContentPage
{
  
    private readonly Rac rac;
    private readonly HttpClient client;
	public Reservation(IStadiumServices stadiumServices,Rac rac)
	{
		InitializeComponent();
        client = new HttpClient();
        this.rac = rac;
        LoadReservationsAsync();
	}
    private async void LoadReservationsAsync()
    {
        try
        {
            var userId = Preferences.Get("UserId", null);
            if (string.IsNullOrEmpty(userId))
            {
                await DisplayAlert("Error", "User ID not found in preferences.", "OK");
                return;
            }

            var response = await client.GetAsync($"http://10.0.2.2:5079/api/booking/user-booking/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", $"Failed to fetch reservations: {response.StatusCode}", "OK");
                return;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var reservations = JsonSerializer.Deserialize<List<ReservationDTO>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (reservations != null)
            {
                PopulateReservations(reservations);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message},\n {ex.StackTrace}", "OK");
        }
    }

    private void PopulateReservations(List<ReservationDTO> reservations)
    {
        ReservationsContainer.Children.Clear();

        foreach (var reservation in reservations)
        {
            // Create a Frame for each reservation
            var reservationFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#003366"),
                CornerRadius = 12,
                Padding = 15,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "Reservation Summary",
                            FontSize = 18,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Colors.White
                        },
                        new BoxView
                        {
                            HeightRequest = 1,
                            Color = Colors.White,
                            Margin = new Thickness(0, 5, 0, 10)
                        },
                        new Label
                        {
                            Text = reservation.PadelPlayground.PlaygroundName.ToString(),
                            TextColor = Colors.White
                        },
                        new Label
                        {
                            Text = $"Date: {reservation.BookingDate:dddd, dd MMM yyyy}",
                            TextColor = Colors.White
                        },
                        new Label
                        {
                            Text = $"Time: {reservation.StartTime.ToString()}",
                            TextColor = Colors.White
                        },
                        new Label
                        {
                            Text = $"Duration: {reservation.Duration.ToString()} minutes",
                            TextColor = Colors.White
                        },
                        new Label
                        {
                            Text = $"Total: {reservation.Price.ToString()} SAR",
                            TextColor = Colors.White,
                            FontAttributes = FontAttributes.Bold
                        }
                    }
                }
            };

            // Add the frame to the container
            ReservationsContainer.Children.Add(reservationFrame);
        }
    }
    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        var stadiumServices = new StadiumServices();
        var rac = new Rac();
        var reservation = new Reservation(stadiumServices,rac);
        await Navigation.PushAsync(new StadiumsPage(
            stadiumServices,reservation,rac));
    }

    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {

       
    }

    private async void ImageButton_Clicked_3(object sender, EventArgs e)
    {
        await Navigation.PushAsync(rac);

    }

    private async void ImageButton_Clicked_4(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Game());

    }
}

public class ReservationDTO
{
    public int BookingId { get; set; }
    public string UserId { get; set; }
    public PadelPlayground PadelPlayground { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal Price { get; set; }
}

public class PadelPlayground
{
    public int Id { get; set; }
    public string PlaygroundName { get; set; }
    public string Coordinates { get; set; }
}