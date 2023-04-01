using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Data;
using WebBook.Models;
using WebBook.Models.Common;
using WebBook.ViewModels;

namespace WebBook.Areas.Admin.Controllers
{ 
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var items = _context.News!.OrderByDescending(x=>x.CreatedDate).ToList();
            return View(items);
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
            news.Alias = Filter.FilterChar(vm.Title);
            if(vm.Image != null)
            {
                news.Image = UploadImage(vm.Image);
            }
                


            news.CreatedDate = DateTime.Now;
            news.CategoryId = 4;
            news.ModifiedDate = DateTime.Now;
          
 
            _context.News!.Add(news);
            _context.SaveChanges();

            return RedirectToAction("Index");
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
                    Alias = Filter.FilterChar(vm.Title!)
                };
                if (vm.Image != null)
                {
                    news.Image = UploadImage(vm.Image);
                }



                news.CreatedDate = DateTime.Now;
                news.CategoryId = 4;
                news.ModifiedDate = DateTime.Now;


                _context.News!.Add(news);
                _context.SaveChanges();

                return RedirectToAction("Index");
       
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
