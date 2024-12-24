namespace SiteVantagePro_API.Domain.Common;
public abstract class BaseAuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTimeOffset UpdatedDate { get; set; }
}
