using System.Diagnostics;
using SiteVantagePro_API.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace SiteVantagePro_API.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IUser user,
        IIdentityService identityService)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _user = user;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Id ?? string.Empty;
            var email = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                email = await _identityService.GetUserEmailAsync(userId);
            }

            _logger.LogWarning("SiteVantagePro_API Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Email} {@Request}",
                requestName, elapsedMilliseconds, userId, email, request);
        }

        return response;
    }
}
