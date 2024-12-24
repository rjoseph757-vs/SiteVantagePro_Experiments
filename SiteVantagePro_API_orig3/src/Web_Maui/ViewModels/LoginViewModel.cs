using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Web_Maui.Services;
using Web_Maui.Views.Pages;

namespace Web_Maui.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _emailAddress = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    private readonly ILoginService _loginService = new LoginService();

    public ICommand LoginCommand { get; private set; }

    public LoginViewModel()
    {
        LoginCommand = new AsyncRelayCommand(Login);
    }

    private async Task Login()
    {
        if (!string.IsNullOrEmpty(EmailAddress) && !string.IsNullOrEmpty(Password))
        {
            var userInfo = await _loginService.LoginServiceAsync(EmailAddress, Password);

            if (Preferences.ContainsKey(nameof(App.LoginInfo)))
            {
                Preferences.Remove(nameof(App.LoginInfo));
            }

            string userDetails = JsonConvert.SerializeObject(userInfo);
            Preferences.Set(nameof(App.LoginInfo), userDetails);
            App.LoginInfo = userInfo;

            await Shell.Current.GoToAsync(nameof(HomePage));
        }
    }
}
