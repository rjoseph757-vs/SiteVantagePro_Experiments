using Microsoft.AspNetCore.Authorization;

namespace SiteVantagePro_API.Infrastructure.Permissions;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

