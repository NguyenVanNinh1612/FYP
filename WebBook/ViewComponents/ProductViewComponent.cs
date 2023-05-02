using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.ViewModels;

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
                var productSale = _context.Products!.Where(x => x.IsHome).Take(12).ToList();
                var listProductSale = new List<ProductVM>();
                foreach(var item in productSale)
                {
                    ProductVM vm = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Discount = item.Discount,
                        //PriceSale = item.PriceSale,
                        Avatar = _context?.ProductImages?.Where(x => x.ProductId == item.Id).ToList()?.FirstOrDefault(x => x.IsAvatar)?.ImageName
                    };
                    listProductSale.Add(vm);
                }
                
                return View("ProductSale", listProductSale);
            }
            else
            {
                var products = _context.Products!.Where(x => x.IsHome).Take(12).ToList();
                var listProducts = new List<ProductVM>();
                foreach (var item in products)
                {
                    ProductVM vm = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Discount = item.Discount,
                        //PriceSale = item.PriceSale,
                        CategorySlug = _context?.Categories?.FirstOrDefault(x => x.Id == item.CategoryId)?.Slug,

                        Avatar = _context?.ProductImages?.Where(x => x.ProductId == item.Id).ToList()?.FirstOrDefault(x => x.IsAvatar)?.ImageName
                    };
                    listProducts.Add(vm);
                }
                return View(listProducts);
            }
        }
    }
}
