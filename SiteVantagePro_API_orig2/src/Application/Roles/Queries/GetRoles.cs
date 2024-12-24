using SiteVantagePro_API.Application.Common.Services.Identity;
using SiteVantagePro_API.WebUI.Shared.AccessControl;

namespace SiteVantagePro_API.Application.Roles.Queries;

public record GetRolesQuery : IRequest<RolesVm>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, RolesVm>
{
    private readonly IIdentityService _identityService;

    public GetRolesQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RolesVm> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return new RolesVm
        {
            Roles = await _identityService.GetRolesAsync(cancellationToken)
        };
    }
}
