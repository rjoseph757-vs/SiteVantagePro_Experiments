using Microsoft.AspNetCore.Authorization;

namespace SiteVantagePro_API.Infrastructure.Permissions;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler() { }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        await Task.Delay(1); //cheat: used to allow async since there is no other await in the method
        if (context.User == null)
        {
            return;
        }
        var permissionss = context.User.Claims.Where(x => x.Type == "Permission" &&
                                                            x.Value == requirement.Permission &&
                                                            x.Issuer == "LOCAL AUTHORITY");
        if (permissionss.Any())
        {
            context.Succeed(requirement);
            return;
        }
    }
}
