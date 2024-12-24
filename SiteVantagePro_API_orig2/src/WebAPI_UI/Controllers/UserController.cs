using Microsoft.AspNetCore.Mvc;

namespace SiteVantagePro_API.WebAPI_UIControllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Claims()
        {
            return View();
        }
    }
}
