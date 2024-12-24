using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SiteVantagePro_API.Domain.Entities;
using WebWasmBlazor.Models;
using WebWasmBlazor.Services.Common;

namespace WebWasmBlazor.Services;

public class WeatherForecastHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ApiConnectionOptions _apiConnectionOptions;
    private readonly ILogger<WeatherForecastHttpClient> _logger;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    //public Uri InstanceUrl { get; set; }
    //public string IssuedAt { get; set; }
    public WeatherForecastHttpClient(HttpClient httpClient,
                                     IOptions<ApiConnectionOptions> apiConnectionOptions,
                                     ILogger<WeatherForecastHttpClient> logger)
    {
        _apiConnectionOptions = apiConnectionOptions.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<WeatherForecast>> GetWeatherForecastsAsync()
    {
        _ = await ClientLoginAsync().ConfigureAwait(false);
        var forecasts = await _httpClient.GetFromJsonAsync<List<WeatherForecast>>("/api/WeatherForecast");
        return forecasts ?? [];
    }

    public async Task<List<WeatherForecast>> GetWeatherForecastsAsync2()
    {
        try
        {
            _ = await ClientLoginAsync().ConfigureAwait(false);
            var response = await _httpClient.GetAsync("/api/WeatherForecasts");
            if (response.IsSuccessStatusCode)
            {
                var weatherForecasts = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
                return weatherForecasts ?? [];
            }
        }
        catch (Exception exception)
        {
            string message = $"Error Logging in to MapIdentityApi Web Api: {exception.Message}";
            _logger.LogError(exception, "{message}", message);
        }
        return [];
    }

    // Attempt to use CacheEngine with Expiration Date one minute less to force a request for a new one
    private async Task<bool> ClientLoginAsync()
    {
        _logger.LogInformation("ClientLoginAsync()");
        string cacheKey = $"ClientLoginAsync()";
        var cachedToken = CacheEngine.Get<MapIdentityApiToken>(cacheKey);
        if (cachedToken == null)
        {
            try
            {
                var token = await GetNewMapIdentityApiToken().ConfigureAwait(false);
                cachedToken = token;
                // MapIdentityApi returns time in seconds, so we are dividing it by 60 to set the minutes value instead of seconds value
                //var issued_at = GetExpiryTimestamp(IssuedAt);
                //var expired_at = issued_at.AddMinutes(10); //MapIdentityApi defaults to 60 minutes in Web API Project
                CacheEngine.Add(cachedToken, cacheKey, 10);
                return cachedToken != null;
            }
            catch (Exception ex)
            {
                string message = $"Error Logging in to MapIdentityApi Web Api: {ex.Message}";
                _logger.LogError(ex, "{message}", message);
                return false;
            }
        }

        AccessToken = cachedToken.AccessToken;
        //InstanceUrl = cachedToken.InstanceUrl;
        //IssuedAt = cachedToken.IssuedAt;
        RefreshToken = cachedToken.RefreshToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        return cachedToken != null;
    }

    private async Task<MapIdentityApiToken> GetNewMapIdentityApiToken()
    {
        var email = _apiConnectionOptions.ApiEmail; //"superadmin@localhost";
        var password = _apiConnectionOptions.ApiPassword; //"SuperAdmin1!";
        var loginResponse = await _httpClient.PostAsJsonAsync("/identity/login", new { email, password });
        //var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        //var accessToken = loginContent.GetProperty("accesstoken").GetString();
        //var refreshToken = loginContent.GetProperty("refreshtoken").GetString();
        var loginContent = await loginResponse.Content.ReadFromJsonAsync<MapIdentityApiToken>();
        if (loginContent != null)
        {
            AccessToken = loginContent!.AccessToken;
            RefreshToken = loginContent!.RefreshToken;
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", AccessToken);
            Console.WriteLine(await _httpClient.GetStringAsync("/requires-auth"));
            return loginContent;
        }

        return loginContent ?? throw new Exception("No Auth");
    }


    //private async Task<HttpResponseMessage> QueryDescribeRecordJson(string objectApiName)
    //{
    //    _ = await ClientLoginAsync().ConfigureAwait(false);
    //    string restQuery = $"{InstanceUrl}{_apiConnectionOptions.ApiEndpoint}sobjects/{objectApiName}/describe";
    //    var request = new HttpRequestMessage(HttpMethod.Get, restQuery);
    //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //    var response = await _httpClient.SendAsync(request);
    //    return response;
    //}

    //private async Task<HttpResponseMessage> CreateRecordJson(HttpClient client, string recordData, string recordType, string recordId)
    //{
    //    _ = await ClientLoginAsync().ConfigureAwait(false);
    //    HttpContent content = new StringContent(recordData, Encoding.UTF8, "application/json");
    //    string soqlQuery = $"{InstanceUrl}{_apiConnectionOptions.ApiEndpoint}sobjects/{recordType}/{recordId}";
    //    var request = new HttpRequestMessage(HttpMethod.Post, soqlQuery);
    //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //    request.Content = content;
    //    var response = await client.SendAsync(request);
    //    return response;
    //}

    //private async Task<HttpResponseMessage> UpdateRecordJson(HttpClient client, string recordData, string recordType, string recordId)
    //{
    //    _ = await ClientLoginAsync().ConfigureAwait(false);
    //    HttpContent content = new StringContent(recordData, Encoding.UTF8, "application/json");
    //    string requestUri = $"{InstanceUrl}{_apiConnectionOptions.ApiEndpoint}sobjects/{recordType}/{recordId}?_HttpMethod=PATCH";
    //    var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
    //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //    request.Content = content;
    //    var response = await client.SendAsync(request);
    //    return response;
    //}




}
