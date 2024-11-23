using PadelChampMobile.Services;


namespace PadelChampMobile;

public partial class LoginPage : ContentPage
{
    readonly ILoginRepository loginRepository = new LoginSeevices();
    readonly IStadiumServices stadiumServices;
    private readonly StadiumsPage stadiumsPage;
    public LoginPage(IStadiumServices stadiumServices,StadiumsPage stadiumsPage)
    {
        InitializeComponent();
        this.stadiumServices = stadiumServices;
        this.stadiumsPage = stadiumsPage;
    }

   

    private async void login_Button_Clicked(object sender, EventArgs e)
    {

        string Email = TxtEmail.Text;
        string password = TxtPassword.Text;
        if (Email == null || password == null)
        {
            await DisplayAlert("warning", "Please Enter Email And Password", "Ok");
            return;
        }
        var t = await loginRepository.Login(Email, password); // ok ==>1
        if (t != "true")
        {
            await DisplayAlert("warning", t, "Ok");
            
        }
        else {
            await Navigation.PushAsync(stadiumsPage);

        }
    }

   
}