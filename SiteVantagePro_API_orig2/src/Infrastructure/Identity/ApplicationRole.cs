using Microsoft.AspNetCore.Identity;
using SiteVantagePro_API.WebUI.Shared.Authorization;

namespace SiteVantagePro_API.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {

    }
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public Permissions Permissions { get; set; }
}

