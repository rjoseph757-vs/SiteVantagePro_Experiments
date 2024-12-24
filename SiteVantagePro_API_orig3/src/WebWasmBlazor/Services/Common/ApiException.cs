namespace WebWasmBlazor.Services.Common;

public class ApiException : Exception
{
    public int StatusCode { get; set; }
    public string Content { get; set; } = string.Empty;
}

