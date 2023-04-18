using Microsoft.AspNetCore.Mvc;

namespace WebBook.Controllers
{
	public class NewsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
