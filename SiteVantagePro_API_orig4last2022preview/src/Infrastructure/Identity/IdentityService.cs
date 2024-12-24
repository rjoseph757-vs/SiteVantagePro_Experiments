using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiteVantagePro_API.Application.Common.Interfaces;
using SiteVantagePro_API.WebUI.Shared.AccessControl;
using SiteVantagePro_API.WebUI.Shared.Authorization;
using SiteVantagePro_API.WebUI.Shared.Common;

namespace SiteVantagePro_API.Infrastructure.Identity;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    public async Task<string> GetUserEmailAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.Email!;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u =>
            u.Id == userId);

        return user.UserName!;
    }

    public async Task<Result<string>> CreateUserAsync(
        string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded ? Result<string>.Success(user.Id) : Result<string>.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u =>
            u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    private async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<IList<RoleDto>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles
            .ToListAsync(cancellationToken);

        var result = roles
            .Select(r => new RoleDto(r.Id, r.Name ?? string.Empty, r.Permissions))
            .OrderBy(r => r.Name)
            .ToList();

        return result;
    }

    public async Task UpdateRolePermissionsAsync(string roleId, Permissions permissions)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        if (role != null)
        {
            role.Permissions = permissions;

            await _roleManager.UpdateAsync(role);
        }
    }

    public async Task<IList<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .OrderBy(r => r.UserName)
            .Select(u => new UserDto(u.Id, u.UserName ?? string.Empty, u.Email ?? string.Empty))
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDto> GetUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        Guard.Against.NotFound(id, user);

        var result = new UserDto(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty);

        var roles = await _userManager.GetRolesAsync(user);

        result.Roles.AddRange(roles);

        return result;
    }

    public async Task UpdateUserAsync(UserDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(updatedUser.Id);

        Guard.Against.NotFound(updatedUser.Id, user);

        user.UserName = updatedUser.UserName;
        user.Email = updatedUser.Email;

        await _userManager.UpdateAsync(user);

        var currentRoles = await _userManager.GetRolesAsync(user);
        var addedRoles = updatedUser.Roles.Except(currentRoles);
        var removedRoles = currentRoles.Except(updatedUser.Roles);

        if (addedRoles.Any())
        {
            await _userManager.AddToRolesAsync(user, addedRoles);
        }

        if (removedRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
        }
    }

    public async Task CreateRoleAsync(RoleDto newRole)
    {
        var role = new IdentityRole { Name = newRole.Name };

        await _roleManager.CreateAsync((ApplicationRole)role);
    }

    public async Task UpdateRoleAsync(RoleDto updatedRole)
    {
        var role = await _roleManager.FindByIdAsync(updatedRole.Id);

        Guard.Against.NotFound(updatedRole.Id, role);

        role.Name = updatedRole.Name;

        await _roleManager.UpdateAsync(role);
    }

    public async Task DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        Guard.Against.NotFound(roleId, role);

        await _roleManager.DeleteAsync(role);
    }
}
