using Microsoft.AspNetCore.Mvc;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
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

            IEnumerable<Category> categories = _context.Categories!.OrderByDescending(x => x.Id);
           
            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(x => x.Title.ToLower().Contains(searchString.ToLower()));
            }
            categories = categories.ToPagedList(pageNumber, pageSize);

            return View(categories);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                //var category = new Category()
                //{
                //    Title = model.Title,
                //    Alias = SeoUrlHelper.FrientlyUrl(model.Title!),
                //    Description = model.Description,
                //    SeoTitle = model.SeoTitle,
                //    SeoDescription = model.SeoDescription,
                //    SeoKeywords = model.SeoKeywords,
                //    Position = model.Position,
                //};

                model.Alias = SeoUrlHelper.FrientlyUrl(model.Title!);
                _context.Categories!.Add(model);
                //_context.Categories!.Add(category);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

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
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = SeoUrlHelper.FrientlyUrl(model.Title!);
                _context.Categories!.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);

            /*if (!ModelState.IsValid)
            {
                return View(model);
            }
            _context.Categories!.Attach(model);
            model.Alias = Filter.FilterChar(model.Title!);
            _context.Entry(model).Property(x => x.Title).IsModified = true;
            _context.Entry(model).Property(x => x.Description).IsModified = true;
            _context.Entry(model).Property(x => x.Alias).IsModified = true;
            _context.Entry(model).Property(x => x.SeoDescription).IsModified = true;
            _context.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
            _context.Entry(model).Property(x => x.SeoTitle).IsModified = true;
            _context.Entry(model).Property(x => x.Position).IsModified = true;
            _context.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
            _context.Entry(model).Property(x => x.ModifiedBy).IsModified = true;

        
            _context.SaveChanges();

            return RedirectToAction("Index");*/
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories!.Find(id);
            if (category != null)
            {
                
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Json(new { success = true });
            }
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
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
