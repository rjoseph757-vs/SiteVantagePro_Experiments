using Microsoft.AspNetCore.Identity;
using SiteVantagePro_API.Domain.Constants;
using SiteVantagePro_API.Infrastructure.Identity;

namespace SiteVantagePro_API.Infrastructure.Data;
public static class ContextSeed
{
    public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new ApplicationRole(RolesConstants.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(RolesConstants.Admin.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(RolesConstants.FieldManager.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(RolesConstants.Vendor.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(RolesConstants.User.ToString()));
        await SeedSuperAdminAsync(userManager);
    }

    public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager)
    {
        //Seed Default User
        var defaultUser = new ApplicationUser
        {
            UserName = "superadmin",
            Email = "superadmin@gmail.com",
            FirstName = "Super",
            LastName = "Admin",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                await userManager.AddToRoleAsync(defaultUser, RolesConstants.User.ToString());
                await userManager.AddToRoleAsync(defaultUser, RolesConstants.Vendor.ToString());
                await userManager.AddToRoleAsync(defaultUser, RolesConstants.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, RolesConstants.SuperAdmin.ToString());
            }
        }
    }
}
