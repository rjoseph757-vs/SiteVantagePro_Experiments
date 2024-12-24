namespace SiteVantagePro_API.WebAPI_UI.Models;

public class PermissionViewModel
{
    public string RoleId { get; set; } = string.Empty;
    public IList<RoleClaimsViewModel> RoleClaims { get; set; } = new List<RoleClaimsViewModel>();
}
