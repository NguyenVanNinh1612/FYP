using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebBook.ViewModels;
using WebBook.Models;
using WebBook.Common;
using WebBook.Data;
using WebBook.Payment;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebBook.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVnPayService _vnPayService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public INotyfService _notifyService { get; }
        public CartController(ApplicationDbContext context, INotyfService notifyService, IVnPayService vnPayService,
            IWebHostEnvironment webHostEnviroment, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _context = context;
            _notifyService = notifyService;
            _vnPayService = vnPayService;
            _webHostEnvironment = webHostEnviroment;
                 _userManager = userManager;
       _emailService = emailService;
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
            ViewBag.ids = ids;
            return View();
        }

        [HttpPost]
        public IActionResult PaymentConfirm(OrderVM vm, string ids)
        {
            if (ids == null)
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
                    if (cartItem != null)
                    {
                        totalPrice += cartItem.TotalPrice;
                    }

                }
            }
            Random rd = new();

           
            Order order = new()
            {
                CustomerName = vm.Name,
                Address = vm.Address + ", " + vm.Ward + ", " + vm.District + ", " + vm.City,
                Email = vm.Email,
                Phone = vm.Phone,
                Quantity = carts.Sum(x => x.Quantity),
                TotalAmount = carts.Sum(x => x.TotalPrice),
                PaymentMethod = vm.PaymentMethod,
                Code = "DH" + DateTime.Now.ToString("yyss") + rd.Next(0, 9),
                Status = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
           
            };
            var loggedInUser = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if(loggedInUser != null)
            {
                order.CreatedBy = loggedInUser.UserName;
            }

            _context.Orders?.Add(order);
            _context.SaveChanges();


            foreach (var item in carts)
            {
                OrderDetail od = new()
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Price = item.TotalPrice,
                    Quantity = item.Quantity,
                };
                _context.OrderDetails?.Add(od);
                _context.SaveChanges();
            }
            var myCart = Carts;
            for(int i=0; i<carts.Count; i++)
            {
                var item = myCart.SingleOrDefault(x=>x.ProductId == carts[i].ProductId);
                if (item != null)
                {
                    myCart.Remove(item);
                }
            }
            HttpContext.Session.Set("GioHang", myCart);

            if (order.PaymentMethod)
            {
                var url = _vnPayService.CreatePaymentUrl(order, HttpContext);
                return Redirect(url);
            }

            //Gui mail 
            var strSanpham = "";
            decimal thanhtien = 0;


            
            foreach(var sp in carts)
            { 
                strSanpham += "<tr>";
                strSanpham += "<td>" + sp.ProductName + "</td>";
                strSanpham += "<td>" + sp.Quantity + "</td>";
                strSanpham += "<td>" + ExtensionHelper.ToVnd(sp.TotalPrice) + "</td>";
                strSanpham += "</tr>";
                thanhtien += sp.TotalPrice;
            }
            string pathContent = System.IO.File.ReadAllText( Path.Combine(_webHostEnvironment.WebRootPath, "templates/mail_contents/send2.html"));
            pathContent = pathContent.Replace("{{MaDon}}", order.Code);
            pathContent = pathContent.Replace("{{SanPham}}", strSanpham);
            pathContent = pathContent.Replace("{{TenKhachHang}}", order.CustomerName);
            pathContent = pathContent.Replace("{{SoDienThoai}}", order.Phone);
            pathContent = pathContent.Replace("{{Email}}", order.Email);
            pathContent = pathContent.Replace("{{DiaChi}}", order.Address);
            pathContent = pathContent.Replace("{{NgayDat}}", order.CreatedDate.ToString("dd/MM/yyyy"));
            pathContent = pathContent.Replace("{{ThanhTien}}", ExtensionHelper.ToVnd(thanhtien));
            pathContent = pathContent.Replace("{{TongTien}}", ExtensionHelper.ToVnd(thanhtien + 30000));
            pathContent = pathContent.Replace("{{PhuongThuc}}", "Thanh toán tiền mặt");
            
            var checkSend = _emailService.Send("VNBOOK", "Đơn hàng #"+order.Code.ToString(), pathContent.ToString(), order.Email);

            if (checkSend)
            {
                return View("CheckOutSuccess");
              
            }            
            else
            {
                _notifyService.Error("Có lỗi xảy ra trong quá trình xử lý");
                return RedirectToAction("CheckOut");
            }
            
        }

        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            //Kiem tra xem thanh toan thanh con hay chua
            if (response.IsPay)
            {
                var order = _context.Orders?.FirstOrDefault(x => x.Id == response.Id);
                if (order != null) order.IsPay = true;
                _context.SaveChanges();


                return View("PaymentSuccess");
            }
            else // neu thanh toan ko thanh cong thi xoa don hang
            {
                var orderDetails = _context.OrderDetails?.Where(x => x.OrderId == response.Id).ToList();
                if (orderDetails.Count > 0)
                {
                    foreach(var item in orderDetails)
                    {
                        _context.OrderDetails.Remove(item);
                    }
                    var order = _context.Orders.FirstOrDefault(x => x.Id == response.Id);
                    if (order != null)
                    {
                        _context.Orders.Remove(order);
                    }

                    _context.SaveChanges();

                }
                return View("PaymentFail");
            }
            //return Json(response);
           
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
                    Discount = product.Discount,
                    //PriceSale = product.PriceSale,
                    MaxQuantity = product.Quantity,
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
