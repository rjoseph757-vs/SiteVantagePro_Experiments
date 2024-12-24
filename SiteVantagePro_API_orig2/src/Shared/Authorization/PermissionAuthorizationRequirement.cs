using Microsoft.AspNetCore.Authorization;

namespace SiteVantagePro_API.WebUI.Shared.Authorization;
public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public PermissionAuthorizationRequirement(Permissions permission)
    {
        Permissions = permission;
    }

    public Permissions Permissions { get; }
}
