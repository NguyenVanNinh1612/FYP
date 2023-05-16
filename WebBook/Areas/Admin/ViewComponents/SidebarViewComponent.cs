using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.Areas.Admin.ViewComponents
{
    [Area("Admin")]
    public class SidebarViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SidebarViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int productId)
        {
            ViewBag.newOrder = _context.Orders?.Where(x => x.Status != 4 && x.Status != 5).ToList().Count;
            ViewBag.unRead = _context.Contact?.Where(x => x.IsRead == false).ToList().Count;
            return View();
        }
    }
}
