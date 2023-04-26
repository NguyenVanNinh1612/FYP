using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.ViewComponents
{
	public class ProductImageViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		public ProductImageViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke(int productId)
		{
			var images = _context.ProductImages.Where(x => x.ProductId == productId).ToList();
			return View(images);
		}
	}
}
