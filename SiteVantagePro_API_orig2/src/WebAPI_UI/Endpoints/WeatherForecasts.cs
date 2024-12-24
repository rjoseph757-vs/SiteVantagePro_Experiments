using SiteVantagePro_API.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using SiteVantagePro_API.WebUI.Shared.WeatherForecasts;

namespace SiteVantagePro_API.Web.Endpoints;

public class WeatherForecasts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization("Administrator")  //.RequireAuthorization("api")
            .MapGet(GetWeatherForecasts);
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(ISender sender)
    {
        return await sender.Send(new GetWeatherForecastsQuery());
    }
}
