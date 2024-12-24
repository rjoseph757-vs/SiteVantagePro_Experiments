using SiteVantagePro_API.Application.Common.Services.Identity;
using SiteVantagePro_API.WebUI.Shared.AccessControl;

namespace SiteVantagePro_API.Application.Roles.Commands;

public record CreateRoleCommand(RoleDto Role) : IRequest;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand>
{
    private readonly IIdentityService _identityService;

    public CreateRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await _identityService.CreateRoleAsync(request.Role);
    }
}
