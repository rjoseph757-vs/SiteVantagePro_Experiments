using Microsoft.AspNetCore.Identity;

namespace SiteVantagePro_API.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
    }
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    //public Permissions Permissions { get; set; }
}

