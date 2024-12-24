using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class RolesController : Controller
{

    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<RolesController> _logger;
    public RolesController(
                RoleManager<ApplicationRole> roleManager,
                ILogger<RolesController> logger
    )
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName)
    {
        if (roleName != null)
        {
            await _roleManager.CreateAsync(new ApplicationRole(roleName.Trim()));
        }
        return RedirectToAction("Index");
    }

    //[HttpGet]
    //public async Task<ActionResult<IList<ApplicationRole>>> ListRolesAsync()
    //{
    //    var roles = await _roleManager.Roles.ToListAsync();
    //    return roles.Count != 0 ? (ActionResult<IList<ApplicationRole>>)Ok(roles) : (ActionResult<IList<ApplicationRole>>)NotFound();
    //}

}

