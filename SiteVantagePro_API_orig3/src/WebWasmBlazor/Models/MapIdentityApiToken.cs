using System.Text.Json.Serialization;

namespace WebWasmBlazor.Models;

public class MapIdentityApiToken
{
    [JsonPropertyName("tokenType")]
    public required string TokenType { get; set; } //"Bearer"

    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; set; }

    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; } //3600

    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; set; }
}
/*
 * 
// 2fa flow will be added later.
// The request body is: { "username": "<username>", "password": "<password>" }
var loginResponse = await httpClient.PostAsJsonAsync("/identity/login", new { username, password });

// loginResponse is similar to the "Access Token Response" defined in the OAuth 2 spec
// {
//   "tokenType": "Bearer",
//   "accessToken": -,
//   "expiresIn": 3600
//   "refreshToken": //   "accessToken": -,
// }
var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
var accessToken = loginContent.GetProperty("access_token").GetString();

httpClient.DefaultRequestHeaders.Authorization = new("Bearer", accessToken);
Console.WriteLine(await httpClient.GetStringAsync("/requires-auth"));
 
 * */
