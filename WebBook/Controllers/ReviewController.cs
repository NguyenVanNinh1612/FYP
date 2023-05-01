using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.Models;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }
        public ReviewController(ApplicationDbContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
           return View();
        }

        [HttpPost]
        public IActionResult Submit(string userId, int productId, string content, int rating)
        {
            var checkUser = _context.ApplicationUser?.FirstOrDefault(x => x.Id == userId);
            if(checkUser == null)
            {
                _notifyService.Error("Chưa đăng nhập!");
                return Json(new { success = false });
            }
            Review review = new()
            {
                Content = content,
                Rating = Convert.ToByte(rating),
                ApplicationUserId = userId,
                ProductId = productId
            };
            _context.Reviews?.Add(review);
            _context.SaveChanges();

            return Json(new { 
                success = true,
                fullName = review?.ApplicationUser?.FullName,
                createdDate = review?.CreatedDate.ToString("dd MMM yyyy") 
            });
        }
    }
}
