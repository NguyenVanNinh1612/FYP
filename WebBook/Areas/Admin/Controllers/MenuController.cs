using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Super, Admin")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public INotyfService _notifyService { get; }
        public MenuController(ApplicationDbContext context, INotyfService notifyService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notifyService = notifyService;
            _userManager = userManager;
        }

        public IActionResult Index(int? page, string searchString, string currentFilter)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            int pageSize = 5;
            int pageNumber = (page ?? 1); // Neu page == null thi tra ve 1       
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            IEnumerable<Menu> menus = _context.Menus!.OrderByDescending(x => x.Id);

            if (!string.IsNullOrEmpty(searchString))
            {
                menus = menus.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }
            menus = menus.ToPagedList(pageNumber, pageSize);

            return View(menus);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Menu model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name!);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CreatedBy = loggedInUser?.FullName;
                model.ModifiedBy = loggedInUser?.FullName;

                await _context.Menus!.AddAsync(model);
                await _context.SaveChangesAsync();
                _notifyService.Success("Menu created successfully!");
                
                return RedirectToAction("Index", "menu", new
                {
                    area = "admin"
                });
            }

            _notifyService.Error("Menu created failed!");
            return View(model);

        }

        public IActionResult Edit(int id)
        {
            var menu = _context.Menus!.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Menu model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name);
                model.ModifiedBy = loggedInUser?.FullName;
                model.ModifiedDate = DateTime.Now;

                _context.Menus.Update(model);
                await _context.SaveChangesAsync();
              
                _notifyService.Success("Menu updated successfully!");
                return RedirectToAction("Index", "menu", new
                {
                    area = "admin"
                });
            }

            _notifyService.Error("Menu updated failed!");
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var menu = _context.Menus!.Find(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                _context.SaveChanges();
                _notifyService.Success("Menu deleted successfully!");

                return Json(new { success = true });
            }

            _notifyService.Error("Menu deleted failed!");
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _context.Menus!.Find(Convert.ToInt32(item));
                        _context.Menus.Remove(obj);
                        _context.SaveChanges();
                    }
                }

                _notifyService.Success("The selected menu has been deleted successfully!");
                return Json(new { success = true });
            }

            _notifyService.Error("The selected menu has been deleted failed!");
            return Json(new { success = false });
        }
    }
}
