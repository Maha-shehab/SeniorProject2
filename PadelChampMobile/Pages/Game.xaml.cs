#if ANDROID
using Android.Content.PM;
#endif

namespace PadelChampMobile
{
    public partial class Game : ContentPage
    {
        public Game()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
#if ANDROID
            MainActivity.Instance?.SetOrientation(ScreenOrientation.Landscape);
#endif
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
#if ANDROID
            MainActivity.Instance?.SetOrientation(ScreenOrientation.Portrait);
#endif
        }
    }
}