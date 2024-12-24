namespace SiteVantagePro_API.Domain.Common;
public class ServerSideSession
{
    public int Id { get; set; }
    public string? Key { get; set; }
    public string? Scheme { get; set; }
    public string? SubjectId { get; set; }
    public string? SessionId { get; set; }
    public string? DisplayName { get; set; }
    public DateTime Created { get; set; }
    public DateTime Renewed { get; set; }
    public DateTime? Expires { get; set; }
    public string? Data { get; set; }
}
