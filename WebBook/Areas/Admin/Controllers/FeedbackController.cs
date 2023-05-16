using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.Payment;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Super")]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public INotyfService _notifyService { get; }
        public FeedbackController(ApplicationDbContext context, IEmailService emailService, INotyfService notifyService)
        {
            _context = context;
            _emailService = emailService;
            _notifyService = notifyService;
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
            var feedback = _context.Contact?.FirstOrDefault(x => x.Id == id);
            feedback.IsRead = isRead;
            _context.SaveChanges();
            return Json(new { success = true });
        }

        public IActionResult View(int id)
        {
            var feedback = _context.Contact?.FirstOrDefault(x => x.Id == id);
            feedback.IsRead = true;
            _context.SaveChanges();
            return View(feedback);
        }
        [HttpPost]
        public IActionResult Reply(IFormCollection form)
        {
            if(form.Count > 0)
            {
                var check = _emailService.Send("VNBOOK", form["subject"], form["content"], form["email"]);
                if (check)
                {
                    _notifyService.Success("Gửi thành công");
                }
                else
                {
                    _notifyService.Error("Gửi thành công");
                }

            }
            return RedirectToAction("View", new {id = form["id"] });
        }
    }
}
