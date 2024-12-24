using Microsoft.Extensions.Logging;
using Web_Maui.ViewModels;
using Web_Maui.Views.Startup;
using Web_Maui.Views.Pages;

namespace Web_Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands.ttf", "FAB");
                    fonts.AddFont("fa-regular.ttf", "FAR");
                    fonts.AddFont("fa-solid.ttf", "FAS");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddSingleton<AboutViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();

            builder.Services.AddSingleton<VendorPage>(); 
            builder.Services.AddSingleton<VendorViewModel>();

            return builder.Build();
        }
    }
}
