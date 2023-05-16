using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, INotyfService notifyService)
        {
            _logger = logger;
            _context = context;
            _notifyService = notifyService;
        }

        [Route("home")]
        [Route("")]
        public IActionResult Index()
        {
            var flashsale = _context.Events?.FirstOrDefault(x => x.Name == "Flash Sale")?.FlashSale?.Split('.');
            ViewBag.ngay = flashsale[0];
            ViewBag.gio = flashsale[1];
            ViewBag.phut = flashsale[2];

            return View();
        }

        [HttpPost]
        [Route("home/subscribe")]
        public IActionResult Subscribe(string email)
        {
            if (email != null)
            {
                var check = _context.Subscribes.FirstOrDefault(x => x.Email == email);
                if (check == null)
                {
                    _context.Subscribes.Add(new Subscribe
                    {
                        Email = email,
                        CreatedDate = DateTime.Now
                    });
                    _context.SaveChanges();
                    _notifyService.Success("Subcribe successfully!");
                    return Json(new {success = true});
                }
                else
                {
                    _notifyService.Error("This email is registered!");
                    return Json(new {success = false});
                }
                
            }
            _notifyService.Error("Subcribe failed!");
            return Json(new { success = false });
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }


        
        public IActionResult OrderLookup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderLookup(IFormCollection file)
        {

            var order = _context.Orders.FirstOrDefault(x => x.Code == file["Code"].ToString() && x.Email == file["Email"].ToString());
            if(order == null)
            {
                _notifyService.Error("Đơn hàng này không tồn tại!");
                return View();
            }
            var orderDetails = _context.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
            var carts = new List<CartItem>();
            foreach (var item in orderDetails)
            {
                CartItem cart = new()
                {
                    ProductId = item.ProductId,
                    ProductName = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Name,
                    ProductImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == item.ProductId && x.IsAvatar).ImageName,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                carts.Add(cart);
            }

            ViewBag.carts = carts;
            return View("~/Views/Account/OrderDetail.cshtml", order);
        }








        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}