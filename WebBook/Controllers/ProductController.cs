using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.ViewModels;
using X.PagedList;

namespace WebBook.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("product")]
        public IActionResult Index(int? page)
        {

            int pageSize = 12;
			int pageNumber = (page ?? 1);

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
            return View(listVM.ToPagedList(pageNumber, pageSize));
        }

        [Route("product/{slug}")]
        public IActionResult ProductCategory(string slug, int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var categoryId = _context.Categories?.FirstOrDefault(x => x.Slug == slug)?.Id;
            if (categoryId != null)
            {
                ViewBag.categoryId = categoryId;
                ViewBag.categoryName = _context.Categories?.Find(categoryId)?.Name;
                ViewBag.categorySlug = slug;
                var products = _context.Products?.Where(x => x.CategoryId == categoryId).ToList();
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
                return View(listVM.ToPagedList(pageNumber, pageSize));
            }
            return NotFound();
        }

        [Route("product/{slug}/{id}")]
        public IActionResult Detail(int id)
        {
            var product = _context.Products?.FirstOrDefault(x => x.Id == id);
           
            if (product != null)
            {
                ProductVM vm = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Slug = product.Slug,
                    Description = product.Description,
                    Price = product.Price,
                    PriceSale = product.PriceSale,
                    Quantity = product.Quantity,
                    Avatar = _context.ProductImages.FirstOrDefault(x => x.ProductId == product.Id && x.IsAvatar)?.ImageName,
                    Detail = product.Detail,
                    CategoryId = product.CategoryId,
                    SupplierId = product.SupplierId,
                    CategoryName = _context.Categories?.FirstOrDefault(x => x.Id == product.CategoryId)?.Name,
                    SupplierName = _context.Suppliers?.FirstOrDefault(x => x.Id == product.SupplierId)?.Name
                };
                ViewBag.categoryName = vm.CategoryName;
                ViewBag.categorySlug = _context.Categories?.FirstOrDefault(x => x.Id == product.CategoryId)?.Slug;
                return View(vm);
            }
            return NotFound();
        }
    }
}
