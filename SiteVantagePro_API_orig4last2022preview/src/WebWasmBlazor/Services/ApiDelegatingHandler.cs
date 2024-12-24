using WebWasmBlazor.Services.Common;

namespace WebWasmBlazor.Services;

public class ApiDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<ApiDelegatingHandler> _logger;

    public ApiDelegatingHandler(
        ILogger<ApiDelegatingHandler> logger
    )
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //"EnsureSuccessStatusCode" does not allow us to "see" content of error, so handle ourselves after call
        //response.EnsureSuccessStatusCode();
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode == false)
        {
            var apiException = new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content
            };
            _logger.LogError($"Failed to run http query request.RequestUri: {request.RequestUri}  apiException.StatusCode:{apiException.StatusCode}  apiException.Content:{apiException.Content}");
            // if error is: {"error":"invalid_grant","error_description":"authentication failure"}, then check login credentials, including SecurityToken 
        }

        return response;
    }
}
