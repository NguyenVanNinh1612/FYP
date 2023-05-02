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