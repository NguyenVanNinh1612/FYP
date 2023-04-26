using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebBook.Common;
using WebBook.Data;


namespace WebBook.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }

        public CartController(ApplicationDbContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }

        public IActionResult Index()
        {
            return View(Carts);
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                var product = _context.Products?.SingleOrDefault(x => x.Id == id);
                item = new CartItem
                {
                    ProductId = id,
                    ProductName = product.Name,
                    ProductImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == id && x.IsAvatar).ImageName,
                    Price = product.Price,
                    PriceSale = product.PriceSale,
                    Quantity = quantity,
                };
                myCart.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set("GioHang", myCart);
            _notifyService.Success("Thêm vào giỏ hàng thành công!");
            return Json(new { success = true, count = Carts.Count });
        }

        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var myCart = Carts;

            var item = myCart.SingleOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity = quantity;
            }

            HttpContext.Session.Set("GioHang", myCart);
            return Json(new
            {
                success = true,
                quantity,
                totalPrice = ExtensionHelper.ToVnd(item.TotalPrice)
            });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                myCart.Remove(item);
            }
            //HttpContext.Session.Set("GioHang", myCart);

            return Json(new
            {
                success = true
            });
        }

    }
}
