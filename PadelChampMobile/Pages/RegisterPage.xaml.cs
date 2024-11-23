using PadelChampMobile.Services;

namespace PadelChampMobile;

public partial class RegisterPage : ContentPage
{
    readonly IregisterRepository registerRepository = new RegisterServices();
    readonly IStadiumServices stadiumServices;
    private readonly LoginPage loginPage;
    //BackgroundColor="{x:StaticResource DashboardBackground}"
    public RegisterPage(IStadiumServices stadiumServices, LoginPage loginPage)
    {
        InitializeComponent();
        this.stadiumServices = stadiumServices;
        this.loginPage = loginPage;
    }
    private async void Register_Button_Clicked(object sender, EventArgs e)
    {

        string Email = txt_Email.Text;
        string Password = txt_Password.Text;
        string FirstName = txt_firstName.Text;
        string LastName = txt_LastName.Text;
        var Gender = myPicker.SelectedItem as string;
        string PhoneNumber = txt_PhoneNumber.Text;
        DateTime DateOfBirth = datePicker.Date;
        string UserName = txt_UserName.Text; 


        if (Email == null)
        {
            DisplayAlert("warning", "Please Enter Email", "Ok");
            return;
        }
        if (Password == null)
        {
            DisplayAlert("warning", "Please Enter   Password", "Ok");
            return;
        }
        if (Gender == null)
        {
            DisplayAlert("warning", "Please Enter Gender ", "Ok");
            return;
        }
        if (FirstName == null)
        {
            DisplayAlert("warning", "Please Enter FirstName ", "Ok");
            return;
        }

        if (LastName == null)
        {
            DisplayAlert("warning", "Please Enter LastName ", "Ok");
            return;
        }

        if (DateOfBirth == DateTime.Now.Date)
        {
            DisplayAlert("warning", "Please Enter Date Of Birth", "Ok");
            return;
        }


        var t = await registerRepository.Register(UserName, Email, Password, PhoneNumber, FirstName, LastName, Gender, DateOfBirth);
        if (t == "true")
        {
            await Navigation.PushAsync(loginPage);


        }
        else
        {
            DisplayAlert("warning", t, "Ok");
        }


    }
    private async void sign_in_clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(loginPage);
    }
}