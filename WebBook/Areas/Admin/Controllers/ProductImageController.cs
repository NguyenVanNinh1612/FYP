using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductImageController : Controller
	{
		private readonly ApplicationDbContext _context;
		public ProductImageController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index(int id)
		{
			var productImages = _context.ProductImages.Where(x => x.ProductId == id).ToList();
			return View(productImages);
		}
	}
}
