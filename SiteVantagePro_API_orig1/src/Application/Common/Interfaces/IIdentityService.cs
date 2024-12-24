using SiteVantagePro_API.Application.Common.Models;

namespace SiteVantagePro_API.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserEmailAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
