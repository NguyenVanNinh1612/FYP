using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using X.PagedList;

namespace WebBook.Controllers
{
	public class BlogController : Controller
	{
		private readonly ApplicationDbContext _context;
		public BlogController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index(int? page)
		{
			int pageSize = 5;
			int pageNumber = page ?? 1;
			var posts = _context.Posts!.OrderByDescending(x=>x.CreatedDate).ToList();
			return View(posts.ToPagedList(pageNumber, pageSize));
		}

		[Route("blog/{slug}/{id}")]
		public IActionResult Detail(int id)
		{
			var post = _context.Posts.FirstOrDefault(x => x.Id == id);
			if(post == null)
			{
				return NotFound();
			}
			return View(post);
		}
	}
}
