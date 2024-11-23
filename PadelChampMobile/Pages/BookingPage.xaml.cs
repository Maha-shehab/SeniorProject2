using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using PadelChampMobile.Models;
using System.Net.Http;
using System.Net.Http.Json;
namespace PadelChampMobile;

public partial class BookingPage : ContentPage
{
    private readonly PaymentPage _paymentPage;
    public string StadiumName { get; set; }
    private readonly HttpClient _httpClient = new HttpClient();
    public ObservableCollection<BookingTime> BookingTimes { get; set; } = new ObservableCollection<BookingTime>();
    public string CurrentYear { get; set; }
    public ObservableCollection<SelectableDate> UpcomingDays { get; set; } = new ObservableCollection<SelectableDate>();
    public ObservableCollection<SelectableDuration> Durations { get; set; } = new ObservableCollection<SelectableDuration>();
    private string _price;

    private BookingDTO booking;
    private async Task<string> GetStripeCheckoutUrl()
    {
        // Replace with your backend endpoint that generates the Stripe Checkout URL
        string backendUrl = "http://10.0.2.2:5079/api/booking/payment-session";
        var response = await _httpClient.PostAsJsonAsync(backendUrl, booking);

        // Parse the response (assuming it returns a JSON object with a 'url' property)
        var stringUrl = await response.Content.ReadAsStringAsync();
        return stringUrl;
    }
    public string Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                OnPropertyChanged(nameof(Price));  // Notify the UI about the change
            }
        }
    }
    public BookingPage(PaymentPage paymentPage,string stadiumName,int stadiumId)
    {
        InitializeComponent();
        _paymentPage = paymentPage;
        this.StadiumName = stadiumName;
        BindingContext = this;

        // Set the booking DTO object
        booking = new BookingDTO()
        {
            PadelPlaygroundId = stadiumId,
            UserId = Preferences.Get("UserId", null)
        };
        // Set the current year
        CurrentYear = DateTime.Now.Year.ToString();

        // Populate UpcomingDays with today and the next two days
        var today = DateTime.Today;
        for (int i = 0; i < 3; i++)
        {
            UpcomingDays.Add(new SelectableDate
            {
                Date = today.AddDays(i),
                IsSelected = false  // Initially, all are unselected (gray)
            });
        }
        Price = "0 SAR";  // Default price
    }
    private void OnDateClicked(object sender, EventArgs e)
    {
        var tappedFrame = (Frame)sender;
        var selectedDate = (SelectableDate)tappedFrame.BindingContext;

        // Mark the clicked date as selected and unselect others
        foreach (var date in UpcomingDays)
        {
            date.IsSelected = date == selectedDate;
        }
        booking.BookingDate = selectedDate.Date;

        LoadBookingTimes(selectedDate.Date);
        
        Durations.Clear();
        //! reset durations ,start time  and price if user clicks on a different date
        booking.Duration = null;
        booking.StartTime = null;
        booking.Price = null;
    }
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync();
    }
    private async void LoadBookingTimes(DateTime date)
    {
        try
        {
            BookingTimes.Clear();  // Clear existing booking times
            // Replace with your actual API endpoint
            var response = await _httpClient.GetStringAsync($"http://10.0.2.2:5079/api/booking/?date={date}");
            var bookingData = JsonConvert.DeserializeObject<List<BookingTime>>(response);

            // Add fetched data to ObservableCollection
            foreach (var booking in bookingData)
            {
                BookingTimes.Add(booking);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., network errors)
            await DisplayAlert("Error", $"Failed to load booking times: {ex.Message}", "OK");
        }
    }

    private void OnBookingTimeClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedBookingTime = (BookingTime)button.BindingContext;

        // Set all booking times to unselected except the clicked one
        foreach (var booking in BookingTimes)
        {
            booking.IsSelected = booking == selectedBookingTime;
        }

        booking.StartTime = selectedBookingTime.StartTime;
        //! Reset duration
        booking.Duration = null;
        booking.Price = null;
        LoadDurations(selectedBookingTime);
        Price = "0 SAR";
    }
    private void LoadDurations(BookingTime selectedBookingTime)
    {
        // Reset the durations list
        Durations.Clear();

        // Add durations from the selected booking time as SelectableDuration objects
        if (selectedBookingTime != null && selectedBookingTime.Durations != null)
        {
            foreach (var duration in selectedBookingTime.Durations)
            {
                Durations.Add(new SelectableDuration { Duration = duration });
            }
        }
    }

    private async void OnBookingClicked(object sender, EventArgs e)
    {
        if (booking.isBookingComplete)
        {
            _paymentPage.booking = booking;
            _paymentPage.StadiumName = StadiumName;
            //var url = await GetStripeCheckoutUrl();
            //DisplayAlert(url.ToString(), url.ToString(), url.ToString());
            //await Launcher.OpenAsync($"https://www.google.com/search?q={Uri.EscapeDataString(url)}");
            Navigation.PushAsync(_paymentPage);
        }
        else
            DisplayAlert("Invalid booking", $"Please select the booking options!", "Ok");
    }
    private void OnDurationClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;  // Cast sender to Button
        var selectedDuration = (SelectableDuration)button.BindingContext;  // Get the BindingContext, which is the selected duration

        // Deselect all durations
        foreach (var duration in Durations)
        {
            duration.IsSelected = false;
        }

        // Select the clicked duration
        selectedDuration.IsSelected = true;

        booking.Duration = selectedDuration.Duration;
        booking.Price = selectedDuration.Duration == 60 ? 100 : 200;
        // Optionally, update the price based on the selected duration
        UpdatePrice(selectedDuration.Duration);
    }

    private void UpdatePrice(int selectedDuration)
    {
        // Set the price based on the selected duration
        Price = selectedDuration == 60 ? "100 SAR" : (selectedDuration == 120 ? "200 SAR" : "0 SAR");
    }

    public class BookingTime : INotifyPropertyChanged
    {
        public string StartTime { get; set; }
        public List<int> Durations { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

public class SelectableDate : INotifyPropertyChanged
{
    public DateTime Date { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
public class SelectableDuration : INotifyPropertyChanged
{
    public int Duration { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
