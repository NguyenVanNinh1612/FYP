﻿using Microsoft.AspNetCore.Mvc;

namespace WebBook.Controllers
{
	public class ContactController : Controller
	{
		[Route("contact")]
		public IActionResult Index()
		{
			return View();
		}
	}
}