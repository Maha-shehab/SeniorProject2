using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

namespace PadelChampMobile
{
    [Activity(Label = "PadelChampMobile", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, Theme = "@style/Maui.SplashTheme")]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;
        }

        public void SetOrientation(ScreenOrientation orientation)
        {
            RequestedOrientation = orientation;
        }
    }
}