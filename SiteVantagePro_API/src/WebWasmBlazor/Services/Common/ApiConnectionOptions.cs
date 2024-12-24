namespace WebWasmBlazor.Services.Common;

public class ApiConnectionOptions
{
    public string Host { get; set; } = string.Empty; //"https://localhost:7087"
    public string LoginEndpoint { get; set; } = string.Empty; //"/user/login"
    public string ApiEmail { get; set; } = string.Empty; //Set in Azure Config
    public string ApiPassword { get; set; } = string.Empty; //Set in Azure Config
}
