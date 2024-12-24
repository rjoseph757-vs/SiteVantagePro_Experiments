using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebWasmBlazor.Components;
using WebWasmBlazor.Identity;
using WebWasmBlazor.Services;
using WebWasmBlazor.Services.Common;

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
        //string apiBaseAddress = builder.Configuration["SiteVantageProApiServerBaseURI"] ?? "https://localhost:7087";
        //--https://localhost:7087/v1/api/WeatherForecasts
        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

        builder.Services.Configure<ApiConnectionOptions>(builder.Configuration.GetSection(nameof(ApiConnectionOptions)));

        var baseHost = builder.Configuration.GetSection("ApiConnection:Host").Value!;

        //builder.Services.AddHttpClient("WebAPI", client =>
        //    client.BaseAddress = new Uri(baseHost));

        builder.Services.AddTransient<ApiDelegatingHandler>();
        builder.Services.AddHttpClient<WeatherForecastHttpClient>(client =>
        {
            client.BaseAddress = new Uri(baseHost);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
        .AddHttpMessageHandler<ApiDelegatingHandler>()
        //.AddPolicyHandler(GetRetryPolicy())
        ;


        // register the cookie handler
        builder.Services.AddScoped<CookieHandler>();

        // set up authorization
        builder.Services.AddAuthorizationCore();

        // register the custom state provider
        builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

        // register the account management interface
        builder.Services.AddScoped(
            sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

        // set base address for default host
        builder.Services.AddScoped(sp =>
            new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:7127") });

        // configure client for auth interactions
        builder.Services.AddHttpClient(
            "Auth",
            opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:7087"))
            .AddHttpMessageHandler<CookieHandler>();


        // Service Client to make API call
        //builder.Services.AddScoped<WeatherForecastHttpClient>();







        await builder.Build().RunAsync();
    }
}
