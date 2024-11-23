using PadelChampMobile.Services;

namespace PadelChampMobile;

public partial class Rac : ContentPage
{
   
    public Rac()
	{
		InitializeComponent();
       
	}
    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        var stadiumServices = new StadiumServices();
        var rac = new Rac();
        var reservation = new Reservation(stadiumServices, rac);
        await Navigation.PushAsync(new StadiumsPage(
            stadiumServices, reservation, rac));
    }

    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new Reservation(new StadiumServices(),new Rac()));

    }

    private async void ImageButton_Clicked_3(object sender, EventArgs e)
    {

    }

    private async void ImageButton_Clicked_4(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Game());

    }

}