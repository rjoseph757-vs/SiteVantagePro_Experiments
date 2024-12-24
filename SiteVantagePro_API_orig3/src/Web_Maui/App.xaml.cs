using Microsoft.Maui.Platform;
using Web_Maui.Handlers;
using Web_Maui.Models;
using Web_Maui.Views.Startup;

namespace Web_Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //Borderless entry
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, View) =>
            {
                if (View is BorderlessEntry)
                {
#if __ANDROID__
                    handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
                }
            });


            //MainPage = new AppShell();
            MainPage = new NavigationPage(new LoginPage());
        }

        public static LoginInfo? LoginInfo { get; set; }
    }
}
