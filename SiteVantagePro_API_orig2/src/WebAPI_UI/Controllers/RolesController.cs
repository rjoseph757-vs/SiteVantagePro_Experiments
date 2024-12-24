using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class RolesController(
        RoleManager<ApplicationRole> roleManager,
        ILogger<RolesController> logger
            ) : ControllerBase
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ILogger<RolesController> _logger = logger;

    //[HttpGet("ListRoles", Name = nameof(ListRolesAsync))]
    [HttpGet]
    public async Task<ActionResult<IList<ApplicationRole>>> ListRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Count != 0 ? (ActionResult<IList<ApplicationRole>>)Ok(roles) : (ActionResult<IList<ApplicationRole>>)NotFound();
    }
}
