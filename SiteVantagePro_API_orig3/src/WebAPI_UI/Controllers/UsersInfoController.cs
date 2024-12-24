using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteVantagePro_API.WebAPI_UI.Models;

namespace SiteVantagePro_API.WebAPI_UIControllers;

//[Authorize(Roles = "SuperAdmin")] // Role-based authorization
//[Authorize] // default
[Authorize(Policy = "api")] //Policy-based authorization
//[ApiController][ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("/")]
public class UsersInfoController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UsersInfoController(SignInManager<ApplicationUser> signInManager , UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Provide an end point to clear the cookie for logout
    [HttpPost("/Users/logoutnew")]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("/Users/manage/infonew")]
    public async Task<ActionResult<InfoResponseFinal>> ManageInfoNew()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return NotFound();
        }
        var infoResponse = new InfoResponseFinal();
        var email = await _userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email.");
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        infoResponse.Email = email;
        infoResponse.IsEmailConfirmed = isEmailConfirmed;

        return Ok(infoResponse);
    }

}

