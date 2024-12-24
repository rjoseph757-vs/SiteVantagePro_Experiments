using Microsoft.AspNetCore.Mvc;

namespace SiteVantagePro_API.WebAPI_UI.Controllers;
public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
