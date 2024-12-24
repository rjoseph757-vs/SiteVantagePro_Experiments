using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Web_Maui.ViewModels
{
    internal class AboutViewModel
    {
        public static string Title => AppInfo.Name;
        public static string Version => AppInfo.VersionString;
        public static string MoreInfoUrl => "https://aka.ms/maui";
        public static string Message => "This app is written in XAML and C# with .NET MAUI.";
        public ICommand ShowMoreInfoCommand { get; }

        public AboutViewModel()
        {
            ShowMoreInfoCommand = new AsyncRelayCommand(ShowMoreInfo);
        }

        async Task ShowMoreInfo() =>
            await Launcher.Default.OpenAsync(MoreInfoUrl);
    }
}
