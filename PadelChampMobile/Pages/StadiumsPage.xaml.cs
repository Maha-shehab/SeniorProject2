using PadelChampMobile.Models;
using PadelChampMobile.Services;

namespace PadelChampMobile;

public partial class StadiumsPage : ContentPage
{
    private readonly IStadiumServices _service;  // Use readonly to ensure the service doesn't change
    private List<Button> buttonsCollection = new(); // Initialize the list
    private List<ImageButton> imageButtons = new();
    private List<PadelStadiumViewModel> stadiums = new();
    private readonly Reservation reservation;
    private readonly Rac rac;
    public StadiumsPage(IStadiumServices service, Reservation reservation, Rac rac)
    {
        InitializeComponent();
        _service = service;  // Assign the service dependency
        LoadStadiums();       // Load stadium data on page creation
        DisplayUserInfo();
        this.reservation = reservation;
        this.rac = rac;
    }
    private void DisplayUserInfo()
    {
        // Retrieve the email and name from Preferences
        string userEmail = Preferences.Get("UserEmail", "Not Available");
        string userName = Preferences.Get("UserName", "Guest");

   
        // Example: Setting a label in XAML
        userNameLabel.Text = $"Welcome, {userName}!";
    }
    // Asynchronously load stadiums from the service
    private async void LoadStadiums()
    {
        var stadiums = await _service.GetAll();
        if (stadiums != null && stadiums.Any())
        {
            BindStadiums(stadiums.ToList());
        }
    }
    private async void SearchButton_Clicked(object sender, EventArgs e)
    {
        string searchText = searchEditor.Text?.ToLower() ?? string.Empty;
        int count = 0;

        foreach (var child in stadiumsContainer.Children)
        {
            if (child is HorizontalStackLayout layout)
            {
                // Find the Button and ImageButton within each HorizontalStackLayout
                var button = layout.Children.OfType<Button>().FirstOrDefault();
                var imageButton = layout.Children.OfType<ImageButton>().FirstOrDefault();

                if (button != null && imageButton != null)
                {
                    // Check if the button's text matches the search criteria
                    bool isVisible = button.Text.ToLower().Contains(searchText);
                    count += isVisible ? 1 : 0;
                    layout.IsVisible = isVisible;  // Hide or show the entire layout
                }
            }
        }

        if (count == 0)
        {
            await DisplayAlert("No stadiums found 😢", "Try searching again or check our list", "OK!");
            ResetStadiumsVisibility();  // Make all stadiums visible again after clicking OK
        }
    }

    // Helper method to reset the visibility of all stadium layouts
    private void ResetStadiumsVisibility()
    {
        foreach (var child in stadiumsContainer.Children)
        {
            if (child is HorizontalStackLayout layout)
            {
                layout.IsVisible = true;
            }
        }
        searchEditor.Text = "";
    }

    private void BindStadiums(List<PadelStadiumViewModel> stadiums)
    {
        stadiumsContainer.Children.Clear(); // Clear previous entries

        foreach (var stadium in stadiums)
        {
            // Create a button dynamically
            var button = new Button
            {
                Text = stadium.PlaygroundName,
                WidthRequest = 300,
                HeightRequest = 80,
                BackgroundColor = Color.FromHex("#2c3b53"),
                CornerRadius = 20
            };
            button.Clicked += (sender, e) => OnStadiumButtonClicked(stadium);

            // Create an image button for location
            var imageButton = new ImageButton
            {
                Source = "stadiumlocation.png",
                WidthRequest = 50,
                HeightRequest = 50,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            imageButton.Clicked += (sender, e) => OpenMap(stadium.PlaygroundName);

            // Create a horizontal layout to contain both buttons
            var horizontalLayout = new HorizontalStackLayout
            {
                Spacing = 10,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(10)
            };

            // Add the buttons to the horizontal layout
            horizontalLayout.Children.Add(button);
            horizontalLayout.Children.Add(imageButton);

            // Add the horizontal layout to the main container
            stadiumsContainer.Children.Add(horizontalLayout);
        }
    }

    // Handle the event when a stadium button is clicked
    private async void OnStadiumButtonClicked(PadelStadiumViewModel stadium)
    {
        // Navigate to a new page or show details based on the selected stadium
        await Navigation.PushAsync(new BookingPage(new PaymentPage(new StadiumServices(),reservation),
            stadium.PlaygroundName,
            stadium.Id));
    }

    // Open the location in Google Maps
    private async void OpenMap(string location)
    {
        string url = $"https://www.google.com/maps/search/?api=1&query={location}";
        await Launcher.OpenAsync(url);
    }
    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {

    }
    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {

        await Navigation.PushAsync(reservation);
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

