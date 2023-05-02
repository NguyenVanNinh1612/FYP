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
    [Authorize(Roles = "Super, Admin")]
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public INotyfService _notifyService;

        public SupplierController(ApplicationDbContext context, INotyfService notifyService, UserManager<ApplicationUser> userManager)
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

            IEnumerable<Supplier> suppliers = _context.Suppliers!.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(searchString))
            {
                suppliers = suppliers.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(suppliers.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.CreatedBy = loggedInUser?.FullName;
                model.ModifiedBy = loggedInUser?.FullName;
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                    
                await _context.Suppliers!.AddAsync(model);
                await _context.SaveChangesAsync();

                _notifyService.Success("Supplier created successfully!");

                return RedirectToAction("Index", "supplier", new
                {
                    area = "admin"
                });
            }

            _notifyService.Error("Supplier created failed!");
            return View(model);

        }

        public IActionResult Edit(int id)
        {
            var supplier = _context.Suppliers!.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier model)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = loggedInUser?.FullName;
                
                _context.Suppliers!.Update(model);
                await _context.SaveChangesAsync();
                _notifyService.Success("Supplier updated successfully!");
                return RedirectToAction("Index", "supplier", new { area = "admin" });
            }

            _notifyService.Error("Supplier updated failed!");
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers!.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
                _notifyService.Success("Supplier deleted successfully!");

                return Json(new { success = true });
            }

            _notifyService.Error("Supplier deleted failed!");
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
                        var obj = _context.Suppliers!.Find(Convert.ToInt32(item));
                        _context.Suppliers.Remove(obj);
                        _context.SaveChanges();
                    }
                }

                _notifyService.Success("The selected supplier has been deleted successfully!");
                return Json(new { success = true });
            }

            _notifyService.Error("The selected supplier has been deleted failed!");
            return Json(new { success = false });
        }
    }
}
