//--https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/
using SiteVantagePro_API.WebAPI_UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SiteVantagePro_API.WebAPI_UIControllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRolesController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            var viewModel = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);
            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesViewModel()
            {
                UserId = userId,
                UserRoles = viewModel
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            //Guard against messing with SuperAdmin and Admin
            //if (user != null && user.UserName == "SuperAdmin@localhost" || user.UserName == "Admin") {
            //    return RedirectToAction("Index", new { userId = id });
            //}
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            //await Seeds.DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);
            return RedirectToAction("Index", new { userId = id });
        }


        //[HttpGet]
        //private async Task<List<string>> GetUserRoles(ApplicationUser user)
        //{
        //    return new List<string>(await _userManager.GetRolesAsync(user));
        //}

        //[HttpGet]
        //public async Task<IActionResult> Manage(string userId)
        //{
        //    ViewBag.userId = userId;
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
        //        return View("NotFound");
        //    }
        //    ViewBag.UserName = user.UserName;
        //    var model = new List<ManageUserRolesViewModel>();
        //    foreach (var role in _roleManager.Roles)
        //    {
        //        var userRolesViewModel = new ManageUserRolesViewModel
        //        {
        //            RoleId = role.Id,
        //            RoleName = role.Name,
        //            Selected = await _userManager.IsInRoleAsync(user, role.Name)
        //        };
        //        model.Add(userRolesViewModel);
        //    }
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return View();
        //    }
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var result = await _userManager.RemoveFromRolesAsync(user, roles);
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Cannot remove user existing roles");
        //        return View(model);
        //    }
        //    result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Cannot add selected roles to user");
        //        return View(model);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
