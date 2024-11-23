using PadelChampMobile.Services;

namespace PadelChampMobile;
public partial class MainPage : ContentPage
{

    readonly IStadiumServices stadiumServices;
    readonly RegisterPage registerPage;
    readonly LoginPage loginPage;
    public MainPage()
    {
        InitializeComponent();
        this.stadiumServices = new StadiumServices();
        var rac = new Rac();
        this.loginPage = new LoginPage(stadiumServices,new StadiumsPage(stadiumServices,
            new Reservation(stadiumServices,rac),rac));

        this.registerPage = new RegisterPage(stadiumServices,this.loginPage);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(registerPage);
    }

}




