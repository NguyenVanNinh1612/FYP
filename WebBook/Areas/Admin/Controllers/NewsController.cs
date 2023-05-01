using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using WebBook.ViewModels;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{ 
    [Area("Admin")]
    [Authorize(Roles = "Super, Admin")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService { get; }
        public NewsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, INotyfService notifyService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _notifyService = notifyService;
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

            IEnumerable<News> news = _context.News!.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(searchString))
            {
                news = news.Where(x => x.Title!.ToLower().Contains(searchString.ToLower()));
            }

            return View(news.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateNewsVM vm)
        {
            if (ModelState.IsValid)
            {
                return View(vm);
            }

            var news = new News();
            news.Id = vm.Id;
            news.Title = vm.Title;
            news.Slug = SeoUrlHelper.FrientlyUrl(vm.Title);
            if(vm.Image != null)
            {
                news.Image = UploadImage(vm.Image);
            }
                


           
            news.CategoryId = 4;
        
          
 
            _context.News!.Add(news);
            _context.SaveChanges();

            return RedirectToAction("Index", "news", new { area = "admin" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var news = _context.News!.FirstOrDefault(x => x.Id == id);
            if(news == null)
            {
                return View();
            }

            var vm = new CreateNewsVM()
            {
                Id = news.Id,
                Title = news.Title,
                Description = news.Description,
                ImageUrl = news.Image!
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(CreateNewsVM vm)
        {
         
                var news = new News
                {
                    Id = vm.Id,
                    Title = vm.Title,
                    Slug = SeoUrlHelper.FrientlyUrl(vm.Title!)
                };
                if (vm.Image != null)
                {
                    news.Image = UploadImage(vm.Image);
                }



             

                _context.News!.Add(news);
                _context.SaveChanges();

                return RedirectToAction("Index", "news", new { area = "admin" });
       
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.News!.Find(id);
            if (item != null)
            {

                _context.News.Remove(item);
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
                if(items!=null && items.Any())
                {
                    foreach(var item in items)
                    {
                        var obj = _context.News!.Find(Convert.ToInt32(item));
                        _context.News.Remove(obj);
                        _context.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new {success = false});
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
