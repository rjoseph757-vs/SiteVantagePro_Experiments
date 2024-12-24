using Microsoft.AspNetCore.Authorization;
using SiteVantagePro_API.Application.Common.Interfaces;
using SiteVantagePro_API.Domain.Constants;

namespace SiteVantagePro_API.Application.TodoLists;
[Authorize(Roles = Domain.Constants.RolesConstants.Admin)]
[Authorize(Policy = PoliciesConstants.CanPurge)]
public record PurgeTodoListsCommand : IRequest;

public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IApplicationDbContext _context;

    public PurgeTodoListsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        _context.TodoLists.RemoveRange(_context.TodoLists);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
