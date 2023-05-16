using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Super")]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var flashsale = _context.Events?.FirstOrDefault(x=>x.Name == "Flash Sale")?.FlashSale?.Split('.');
            ViewBag.ngay = flashsale[0];
            ViewBag.gio = flashsale[1];
            ViewBag.phut = flashsale[2];
            return View();
        }

        [HttpPost]
        public IActionResult FlashSale(IFormCollection form)
        {
            if (form.Count > 0)
            {

                string flashsale = form["ngay"] + "." + form["gio"] + "." + form["phut"];
                var e =  _context.Events?.FirstOrDefault(x => x.Name == "Flash Sale");
                e.FlashSale = flashsale;
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
    }
}
