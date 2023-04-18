using Microsoft.AspNetCore.Mvc;
using WebBook.Data;


namespace WebBook.Areas.Admin.ViewComponents
{
    [Area("Admin")]
    public class ProductImageViewComponent : ViewComponent  
    {
        private readonly ApplicationDbContext _context;

        public ProductImageViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int productId)
        {
            var productImages = _context.ProductImages.Where(x => x.ProductId == productId).ToList();
            return View(productImages);
        }
    }
}
