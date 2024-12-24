using Newtonsoft.Json;
using Web_Maui.Models;

namespace Web_Maui.Services;
public class LoginService : ILoginService
{
    async Task<LoginInfo?> ILoginService.LoginServiceAsync(string email, string password)
    {
        try
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                _ = new LoginInfo();
                var client = new HttpClient();

                string url = $"https://api/Login/LoginUser/{email}/{password}";
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(""); //.ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    LoginInfo? loginInfo = JsonConvert.DeserializeObject<LoginInfo>(content);
                    return await Task.FromResult(loginInfo);
                }
                else
                {
                    return null;
                }
            }
        }
        catch (global::System.Exception)
        {
            throw;
        }
        return null;
    }
}
