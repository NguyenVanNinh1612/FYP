using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Super, Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public INotyfService _notifyService;

        public CategoryController(ApplicationDbContext context, INotyfService notifyService, UserManager<ApplicationUser> userManager)
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

            IEnumerable<Category> categories = _context.Categories!.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name!);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CreatedBy = loggedInUser?.FullName;
                model.ModifiedBy = loggedInUser?.FullName;

                await _context.Categories!.AddAsync(model);
                await _context.SaveChangesAsync();
                _notifyService.Success("Category created successfully!");

                return RedirectToAction("Index", "category", new
                {
                    area = "admin"
                });
            }

            _notifyService.Error("Category created failed!");
            return View(model);

        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories!.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name!);
                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = loggedInUser?.FullName;

                _context.Categories!.Update(model);
                await _context.SaveChangesAsync();

                _notifyService.Success("Category updated successfully!");
                return RedirectToAction("Index", "category", new
                {
                    area = "admin"
                });
            }

            _notifyService.Error("Category updated failed!");
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories!.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                _notifyService.Success("Category deleted successfully!");

                return Json(new { success = true });
            }

            _notifyService.Error("Category deleted failed!");
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
                        var obj = _context.Categories!.Find(Convert.ToInt32(item));
                        _context.Categories.Remove(obj);
                        _context.SaveChanges();
                    }
                }

                _notifyService.Success("The selected category has been deleted successfully!");
                return Json(new { success = true });
            }

            _notifyService.Error("The selected category has been deleted failed!");
            return Json(new { success = false });
        }
    }
}
