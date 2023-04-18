using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.ViewComponents
{
	public class CategoryViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		public CategoryViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke(int? categoryId)
		{
			if(categoryId != null)
			{
				ViewBag.categoryId = categoryId;
			}
			var categories = _context.Categories?.OrderByDescending(x => x.Id).ToList();
			return View(categories);
		}
	}
}
