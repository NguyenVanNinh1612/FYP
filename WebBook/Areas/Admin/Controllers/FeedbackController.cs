using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Super")]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var listFeedback = _context.Contact.OrderByDescending(x=>x.Id).ToList();
            return View(listFeedback.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public IActionResult Read(int id, bool isRead)
        {
            var feedback = _context.Contact.FirstOrDefault(x => x.Id == id);
            feedback.IsRead = isRead;
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
