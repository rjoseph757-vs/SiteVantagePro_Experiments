//Bug fixed: See note from Ivan de Wit at bottom:--https://codewithmukesh.com/blog/permission-based-authorization-in-aspnet-core/#List_of_all_Registered_Users
using Microsoft.AspNetCore.Identity;
using SiteVantagePro_API.WebAPI_UI.Models;
using System.Reflection;
using System.Security.Claims;

namespace SiteVantagePro_API.WebAPI_UI.Infrastructure;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleId)
    {
        FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (FieldInfo fi in fields)
        {
            allPermissions.Add(new RoleClaimsViewModel { Value = fi?.GetValue(null)?.ToString() ?? "", Type = "Permissions" });
        }
    }
    public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }

}
