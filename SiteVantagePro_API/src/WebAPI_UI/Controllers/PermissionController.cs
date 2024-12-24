﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SiteVantagePro_API.Domain.Constants;
using SiteVantagePro_API.WebAPI_UI.Models;

namespace SiteVantagePro_API.WebAPI_UI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PermissionController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public PermissionController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is not null)
            {
                model.RoleId = roleId;
                var claims = await _roleManager.GetClaimsAsync(role);
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = claims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                    }
                }
                model.RoleClaims = allPermissions;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", new { roleId });
            }
        }

        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role!);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role!, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role!, claim.Value);
            }
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}
