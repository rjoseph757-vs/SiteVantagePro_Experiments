using SiteVantagePro_API.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using SiteVantagePro_API.Domain.Entities;

namespace SiteVantagePro_API.Web.Endpoints;

public class WeatherForecasts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        //app.MapGroup(nameof(WeatherForecasts))
        app.MapGroup("v1/api/WeatherForecasts")
         .RequireAuthorization("api")
         .MapGet(GetWeatherForecasts);
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(ISender sender)
    {
        return await sender.Send(new GetWeatherForecastsQuery());
    }
}
