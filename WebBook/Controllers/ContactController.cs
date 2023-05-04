using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.Controllers
{
	public class ContactController : Controller
	{
		private readonly ApplicationDbContext _context;
		public INotyfService _notifyService { get; set; }
		public ContactController(ApplicationDbContext context, INotyfService notifyService)
		{
			_context = context;
			_notifyService = notifyService;
		}

		[Route("contact")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Submit(IFormCollection form)
		{
			if(form.Count > 0)
			{
				Contact contact = new()
				{
					Name = form["name"],
					Email = form["email"],
					Message = form["message"],
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now

				};
				_context.Contact?.Add(contact);
				_context.SaveChanges();
				_notifyService.Success("Submit successfully!");
				return View("Index");
			}
			
			_notifyService.Error("Submit failed!");
			return View("Index");
		}
		
	}
}
