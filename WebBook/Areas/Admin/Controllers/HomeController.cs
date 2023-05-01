using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Super, Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
