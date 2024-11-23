using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using PadelChampMobile.Services;

namespace PadelChampMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddScoped(typeof(IStadiumServices), typeof(StadiumServices));
        builder.Services.AddSingleton<MainPage>();
        //builder.Services.UseStripe();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .Services.AddTransient<StadiumsPage>().AddTransient<BookingPage>()
            .AddTransient<Game>().AddTransient<LoginPage>().AddTransient<RegisterPage>()
            .AddTransient<Reservation>().AddTransient<Rac>().AddTransient<Error>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
