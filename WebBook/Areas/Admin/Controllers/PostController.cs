using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Data;
using WebBook.Models;
using WebBook.Models.Common;
using WebBook.ViewModels;

namespace WebBook.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PostController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var items = _context.Posts!.OrderByDescending(x => x.CreatedDate).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var posts = new Post();
            posts.Id = vm.Id;
            posts.Title = vm.Title;
            posts.Alias = Filter.FilterChar(vm.Title);
            if (vm.Image != null)
            {
                posts.Image = UploadImage(vm.Image);
            }

            posts.CreatedDate = DateTime.Now;
            posts.CategoryId = 4;
            posts.ModifiedDate = DateTime.Now;

            await _context.Posts!.AddAsync(posts);
            await _context.SaveChangesAsync();

  
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var posts = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);
            if (posts == null)
            {
                return View();
            }

            var vm = new CreatePostVM()
            {
                Id = posts.Id,
                Title = posts.Title,
                Description = posts.Description,
                ImageUrl = posts.Image!
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var posts = new Post();
            posts.Id = vm.Id;
            posts.Title = vm.Title;
            posts.Alias = Filter.FilterChar(vm.Title!);
            if (vm.Image != null)
            {
                posts.Image = UploadImage(vm.Image);
            }



            posts.CreatedDate = DateTime.Now;
            posts.CategoryId = 4;
            posts.ModifiedDate = DateTime.Now;


            await _context.Posts!.AddAsync(posts);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Posts!.Find(id);
            if (item != null)
            {

                _context.Posts.Remove(item);
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
                        var obj = _context.Posts!.Find(Convert.ToInt32(item));
                        _context.Posts.Remove(obj);
                        _context.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
