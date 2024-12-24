using SiteVantagePro_API.Infrastructure.Identity;

namespace SiteVantagePro_API.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapIdentityApi<ApplicationUser>();
    }
}
