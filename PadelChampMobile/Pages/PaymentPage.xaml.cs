using Newtonsoft.Json;
using PadelChampMobile.Models;
using PadelChampMobile.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace PadelChampMobile;

public partial class PaymentPage : ContentPage
{
    private readonly IStadiumServices stadiumServices;
    private readonly Reservation reservation;
    private readonly HttpClient httpClient = new HttpClient();
    public BookingDTO booking { get; set; }
    public string CheckoutURL { get; set; }
    public string StadiumName { get; set; }


    public PaymentPage(IStadiumServices stadiumServices, Reservation reservation)
    {
        InitializeComponent();
        this.stadiumServices = stadiumServices;
        this.reservation = reservation;        
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        string checkoutUrl = await GetStripeCheckoutUrl();

        if (!string.IsNullOrEmpty(checkoutUrl))
        {
            //var uri = new Uri(checkoutUrl);
            //CheckoutURL = checkoutUrl.ToString();
            //await Launcher.OpenAsync(uri);
            //DisplayAlert(checkoutUrl,booking.ToString(),checkoutUrl);
            StripeWebView.Source = checkoutUrl; // Load the URL in WebView
        }
        else
        {
            await DisplayAlert("Error", "Unable to initiate payment. Please try again.", "OK");
        }
    }

    private async Task<string> GetStripeCheckoutUrl()
    {
        // Replace with your backend endpoint that generates the Stripe Checkout URL
        string backendUrl = "http://10.0.2.2:5079/api/booking/payment-session";
        var response = await httpClient.PostAsJsonAsync(backendUrl, booking);

        // Parse the response (assuming it returns a JSON object with a 'url' property)
        var stringUrl = await response.Content.ReadAsStringAsync();
        //var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
        return stringUrl;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(reservation);
    }
}