using SiteVantagePro_API.Application.Common.Services.Identity;
using SiteVantagePro_API.WebUI.Shared.Authorization;

namespace SiteVantagePro_API.Application.AccessControl.Commands;

public record UpdateAccessControlCommand(string RoleId, Permissions Permissions) : IRequest;

public class UpdateAccessControlCommandHandler : IRequestHandler<UpdateAccessControlCommand>
{
    private readonly IIdentityService _identityService;

    public UpdateAccessControlCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(UpdateAccessControlCommand request, CancellationToken cancellationToken)
    {
        await _identityService.UpdateRolePermissionsAsync(request.RoleId, request.Permissions);
    }
}
