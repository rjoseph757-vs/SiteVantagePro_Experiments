using SiteVantagePro_API.WebUI.Shared.AccessControl;
using SiteVantagePro_API.WebUI.Shared.Authorization;
using SiteVantagePro_API.WebUI.Shared.Common;

namespace SiteVantagePro_API.Application.Common.Services.Identity;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<Result<string>> CreateUserAsync(
        string userName,
        string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<IList<RoleDto>> GetRolesAsync(CancellationToken cancellationToken);

    Task UpdateRolePermissionsAsync(string roleId, Permissions permissions);

    Task<IList<UserDto>> GetUsersAsync(CancellationToken cancellationToken);

    Task<UserDto> GetUserAsync(string id);

    Task UpdateUserAsync(UserDto updatedUser);

    Task CreateRoleAsync(RoleDto newRole);

    Task UpdateRoleAsync(RoleDto updatedRole);

    Task DeleteRoleAsync(string roleId);
    Task<bool> IsInRoleAsync(string id, string v);
    Task<string> GetUserEmailAsync(string userId);
    Task<bool> AuthorizeAsync(string id, string policy);
}
