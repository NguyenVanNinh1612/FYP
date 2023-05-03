using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using WebBook.ViewModels;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Super")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }
        public OrderController(ApplicationDbContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1); // Neu page == null thi tra ve 1       
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            var listOrders = new List<Order>();
            listOrders = _context.Orders!.ToList();

            return View(listOrders.OrderByDescending(x=>x.Id).ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Detail(int id)
        {
            ViewBag.order = _context.Orders.First(x => x.Id == id);

            var orderDetails = new List<OrderDetailVM>();
            var listOrderDetails = _context.OrderDetails.Where(x => x.OrderId == id).ToList();
            foreach(var item in listOrderDetails)
            {
                OrderDetailVM vm = new()
                {
                    OrderId = id,
                    ProductId = item.ProductId,
                    ProductName = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Name,
                    //Price = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).PriceSale > 0 ?
                    //_context.Products.FirstOrDefault(x => x.Id == item.ProductId).PriceSale :
                    //_context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price,
                    Price = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price -
                    (_context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price * 
                    (decimal)0.01 * _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Discount),
                    Quantity = item.Quantity,
                    TotalPrice = item.Price
                };
                orderDetails.Add(vm);
            }

            return View(orderDetails);
        }


        [HttpPost]
        public IActionResult ChangeStatus(int id, int status)
        {
            var order = _context.Orders.First(x => x.Id == id);
            if (order != null)
            {
                
                if(status>=1 && status<=4)
                { 
                    if(status == 4)
                    {
                        var orderdetails = _context.OrderDetails?.Where(x => x.OrderId == order.Id).ToList();
                        
                        foreach(var item in orderdetails)
                        {
                            var product = _context.Products?.FirstOrDefault(x => x.Id == item.ProductId);
                            product!.Quantity -= item.Quantity;
                        }
                        _context.SaveChanges();
                    }
                    order.Status = status;
                    _notifyService.Success("Thay đổi trạng thái thành công");
                }
                else if (status == 5)
                {
                    if (order.IsPay)
                    {
                        _notifyService.Error("Đơn hàng đã thanh toán không thể hủy");
                        return Json(new { success = false });
                    }
                    if (order.Status == 3)
                    {
                        _notifyService.Error("Đơn hàng đang giao không thể hủy");
                        return Json(new { success = false });
                    }
                    else if (order.Status == 4)
                    {
                        _notifyService.Error("Đơn hàng đã giao không thể hủy");
                        return Json(new { success = false });
                    }
                    else
                    {
                        order.Status = status;
                        _notifyService.Success("Thay đổi trạng thái thành công");
                    }
                }
            }
            _context.SaveChanges();
            return Json(new {success = true, status = ExtensionHelper.Status(status) });
        }

        public IActionResult Print(int id)
        {
            ViewBag.order = _context.Orders.First(x => x.Id == id);

            var orderDetails = new List<OrderDetailVM>();
            var listOrderDetails = _context.OrderDetails.Where(x => x.OrderId == id).ToList();
            foreach (var item in listOrderDetails)
            {
                OrderDetailVM vm = new()
                {
                    OrderId = id,
                    ProductId = item.ProductId,
                    ProductName = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Name,
                    //Price = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).PriceSale > 0 ?
                    //_context.Products.FirstOrDefault(x => x.Id == item.ProductId).PriceSale :
                    //_context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price,
                    Price = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price -
                    (_context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price *
                    (decimal)0.01 * _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Discount),
                    Quantity = item.Quantity,
                    TotalPrice = item.Price
                };
                orderDetails.Add(vm);
            }

            return View(orderDetails);
        }
    }
}
