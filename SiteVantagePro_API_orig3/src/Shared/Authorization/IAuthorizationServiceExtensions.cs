using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SiteVantagePro_API.WebUI.Shared.Authorization;
public static class IAuthorizationServiceExtensions
{
    public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, Permissions permissions)
    {
        return service.AuthorizeAsync(user, PolicyNameHelper.GeneratePolicyNameFor(permissions));
    }
}
