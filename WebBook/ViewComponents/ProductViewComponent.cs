using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ProductViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string name)
        {
            if (name == "ProductSale")
            {
                var productSale = _context.Products.Where(x => x.IsSale).Take(12).ToList();
                return View("ProductSale", productSale);
            }
            else
            {
                var products = _context.Products.Where(x => x.IsHome).Take(12).ToList();
                return View(products);
            }
        }
    }
}
