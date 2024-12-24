using SiteVantagePro_API.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace SiteVantagePro_API.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    {
        _logger = logger;
        _user = user;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.Id ?? string.Empty;
        string? email = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            email = await _identityService.GetUserEmailAsync(userId);
        }

        _logger.LogInformation("SiteVantagePro_API Request: {Name} {@UserId} {@Email} {@Request}",
            requestName, userId, email, request);
    }
}
