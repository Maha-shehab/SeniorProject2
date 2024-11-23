using PadelChampMobile.Services;
using Microsoft.Maui;
namespace PadelChampMobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        //
    }

    // This method is called when the app is launched via a deep link
    protected override void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);

        if (uri != null && uri.AbsolutePath.Contains("success"))
        {
            // Navigate to the home page or other page upon successful payment
            MainPage = new NavigationPage(new Reservation(new StadiumServices(),new Rac()));
        }
    }
}
