using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebBook.ViewModels;
using WebBook.Models;
using WebBook.Common;
using WebBook.Data;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

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

        [Route("cart")]
        public IActionResult Index()
        {
            return View(Carts);
        }

        [Route("cart/checkout")]
        public IActionResult CheckOut(string ids)
        {
            if(ids == null)
            {
                return View();
            }
            var items = ids.Split(',');
            var carts = new List<CartItem>();
            decimal totalPrice = 0;
            if (items != null)
            {
                foreach (var item in items)
                {
                    var cartItem = Carts.SingleOrDefault(x => x.ProductId == Convert.ToInt32(item));
                    carts.Add(cartItem!);
                    if(cartItem != null)
                    {
                        totalPrice += cartItem.TotalPrice;
                    }
                   
                }
            }
            ViewBag.totalPrice = totalPrice;
            ViewBag.carts = carts;
            // return PartialView("_CartPartial", carts);
            return View();
        }

        [HttpPost]
        public IActionResult PaymentConfirm(OrderVM vm)
        {
           
            var carts = (List<CartItem>)ViewBag.carts;
            Random rd = new();
            Order order = new()
            {
                CustomerName = vm.Name,
                Address = vm.City + ", " + vm.District + ", " + vm.Ward + ", " + vm.Address,
                Phone = vm.Phone,
                TotalAmount = carts.Sum(x => (x.TotalPrice)),
                PaymentMethod = vm.PaymentMethod,
                Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9)
            };

            _context.Orders.Add(order);
          
            foreach(var item in carts)
            {
                OrderDetail od = new()
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity

                };
                _context.OrderDetails.Add(od);
            }
            
             
            return View("Index");
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
                    ProductName = product!.Name,
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
            HttpContext.Session.Set("GioHang", myCart);
            _notifyService.Success("Xóa sản phẩm thành công!");
            return Json(new
            {
                success = true
            });
        }


        public IActionResult TotalPrice(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                decimal total = 0;
                if (items != null)
                {
                    var myCart = Carts;
                    foreach (var item in items)
                    {
                        var cartItem = myCart.SingleOrDefault(x => x.ProductId == Convert.ToInt32(item));
                        total += cartItem.TotalPrice;
                    }

                }
                return Json(new { success = true, t = ExtensionHelper.ToVnd(total) });
            }
            return Json(new { success = false });
        }

    }
}
