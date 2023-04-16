using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public MenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string name)
        {
            if (name == "Menu")
            {
                var menus = _context.Menus!.OrderBy(x => x.Position).ToList();
                return View("Default", menus);
            }
            else if(name == "MenuArrivals")
            {
                var menuArrivals = _context.Categories!.ToList();
                return View("MenuArrivals", menuArrivals);
            }
            else
            {
                var categories = _context.Categories!.ToList();
                return View("Category", categories);
            }
        }

       

    }
}
