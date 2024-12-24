﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteVantagePro_API.WebAPI_UIControllers;

[Authorize(Roles = "SuperAdmin")] // Role-based authorization
[ApiController][ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "This is a protected resource." });
    }

    // Other endpoints
}

