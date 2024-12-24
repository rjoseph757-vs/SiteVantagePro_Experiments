using SiteVantagePro_API.Application.Common.Interfaces;
using SiteVantagePro_API.Application.TodoLists.Queries.GetSites;

namespace SiteVantagePro_API.Application.SiteLists.Queries.GetSites;

public record GetSitesQuery : IRequest<SitesVm>
{
}

public class GetSitesQueryValidator : AbstractValidator<GetSitesQuery>
{
    public GetSitesQueryValidator()
    {
    }
}

public class GetSitesQueryHandler : IRequestHandler<GetSitesQuery, SitesVm>
{
    private readonly IApplicationDbContext _context;

    public GetSitesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SitesVm> Handle(GetSitesQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(1); //only used so I can comment out below when needed ... after initial deployment
        throw new NotImplementedException();
    }
}
