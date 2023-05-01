using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.ViewComponents
{
    public class PostViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public PostViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var posts = _context.Posts.OrderByDescending(x => x.CreatedDate).Take(3).ToList();

            return View(posts);
        }
    }
}
