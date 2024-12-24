using SiteVantagePro_API.Application.Common.Interfaces;

namespace SiteVantagePro_API.Application.SiteLists.Commands.CreateSiteList;

public record CreateSiteListCommand : IRequest<string>
{
}

public class CreateSiteListCommandValidator : AbstractValidator<CreateSiteListCommand>
{
    public CreateSiteListCommandValidator()
    {
    }
}

public class CreateSiteListCommandHandler : IRequestHandler<CreateSiteListCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CreateSiteListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateSiteListCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1); //only used so I can comment out below when needed ... after initial deployment
        throw new NotImplementedException();
    }
}
