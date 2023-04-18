using Microsoft.AspNetCore.Mvc;

namespace WebBook.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
