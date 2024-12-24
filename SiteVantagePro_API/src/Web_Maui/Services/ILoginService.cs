using Web_Maui.Models;

namespace Web_Maui.Services;
public interface ILoginService
{
    Task<LoginInfo?> LoginServiceAsync(string email, string password);
}
