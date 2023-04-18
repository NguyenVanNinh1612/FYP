using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products?.ToList();
            var listVM = new List<ProductVM>();
            foreach (var item in products)
            {
                ProductVM vm = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    PriceSale = item.PriceSale,
                    Avatar = _context.ProductImages.Where(x => x.ProductId == item.Id).ToList().FirstOrDefault(x => x.IsAvatar)?.ImageName,
                    CategorySlug = _context.Categories?.FirstOrDefault(x => x.Id == item.CategoryId)?.Slug,
                    Slug = item.Slug

                };
                listVM.Add(vm);
            }
            return View(listVM);
        }

        [Route("{controller}/{slug?}")]
        public IActionResult ProductCategory(string slug)
        {
            var categoryId = _context.Categories?.FirstOrDefault(x => x.Slug == slug)?.Id;
            ViewBag.categoryId = categoryId;
            ViewBag.categoryName = _context.Categories?.Find(categoryId)?.Name;
            ViewBag.categorySlug = slug;
            var products = _context.Products?.Where(x=>x.CategoryId == categoryId).ToList();
            var listVM = new List<ProductVM>();
            foreach (var item in products)
            {
                ProductVM vm = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    PriceSale = item.PriceSale,
                    Avatar = _context.ProductImages.Where(x => x.ProductId == item.Id).ToList().FirstOrDefault(x => x.IsAvatar)?.ImageName,
                    CategorySlug = _context.Categories?.FirstOrDefault(x => x.Id == item.CategoryId)?.Slug,
                    Slug = item.Slug

                };
                listVM.Add(vm);
            }
            return View(listVM);
        }
    }
}
