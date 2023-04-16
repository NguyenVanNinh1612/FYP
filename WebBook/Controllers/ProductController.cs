using Microsoft.AspNetCore.Mvc;

namespace WebBook.Controllers
{
    public class ProductController :Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
