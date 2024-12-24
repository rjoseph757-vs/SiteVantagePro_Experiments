using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SiteVantagePro_API.Application.Common.Services.Identity;
using SiteVantagePro_API.Domain.Common;

namespace SiteVantagePro_API.Infrastructure.Data.Interceptors;
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;

    public AuditableEntitySaveChangesInterceptor(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AuditEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AuditEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void AuditEntities(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUser.UserId;
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added ||
                entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedBy = _currentUser.UserId;
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }
        }
    }
}
