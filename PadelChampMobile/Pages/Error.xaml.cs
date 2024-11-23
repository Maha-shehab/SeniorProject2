namespace PadelChampMobile;

public partial class Error : ContentView
{
	public Error()
	{
		InitializeComponent();
	}
    private async void onclickgotologin(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Login");
    }
}