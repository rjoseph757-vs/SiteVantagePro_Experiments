using System.Security.Claims;
using SiteVantagePro_API.Application.Common.Services.Identity;

namespace SiteVantagePro_API.WebAPI_UIServices;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?
                            .User?
                            .FindFirstValue(ClaimTypes.NameIdentifier);
}

