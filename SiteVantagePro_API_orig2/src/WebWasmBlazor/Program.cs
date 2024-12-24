using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebWasmBlazor.Services;

namespace WebWasmBlazor;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        // Host and port where external API is hosted
        string apiBaseAddress = builder.Configuration["TMGWebApiServerBaseURI"] ?? "https://localhost:5001";
        //--https://localhost:5001/v1/api/WeatherForecasts
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

        // Service Client to make API call
        builder.Services.AddScoped<WeatherApiService>();

        await builder.Build().RunAsync();
    }
}
