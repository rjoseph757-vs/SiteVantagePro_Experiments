using System.Net.Http.Json;
using WebWasmBlazor.Models;

namespace WebWasmBlazor.Services;

public class WeatherApiService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private string? exceptionMessage;

    public async Task<List<WeatherForecast>?> GetWeatherForecastsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/v1/api/WeatherForecasts");

            if (response.IsSuccessStatusCode)
            {
                var weatherForecasts = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
                return weatherForecasts;
            }
        }
        catch (NotSupportedException exception)
        {
            exceptionMessage = exception.Message;
        }
        return new List<WeatherForecast>();
    }
}
