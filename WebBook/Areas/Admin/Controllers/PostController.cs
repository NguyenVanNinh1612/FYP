using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using WebBook.Utilites;
using WebBook.ViewModels;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Super, Admin")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(ApplicationDbContext context,
            INotyfService notification,
            IWebHostEnvironment webHostEnviroment,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notifyService = notification;
            _webHostEnvironment = webHostEnviroment;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int? page, string searchString, string currentFilter)
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

            var listOfPosts = _context.Posts.ToList();
            //var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

            
            var listOfPostVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                Image = x.Image,
                CreatedBy = x.CreatedBy,
                ModifiedBy = x.ModifiedBy

            }).ToList();


            if (!string.IsNullOrEmpty(searchString))
            {
                listOfPostVM = listOfPostVM.Where(x => x.Title!.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return View(listOfPostVM.OrderByDescending(x=>x.ModifiedDate).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                ShortDescription = vm.ShortDescription,
                ApplicationUserId = loggedInUser!.Id,
                CreatedBy = loggedInUser.FullName,
                ModifiedBy = loggedInUser.FullName,
                Slug = SeoUrlHelper.FrientlyUrl(vm.Title!),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (vm.FileImage != null)
            {
                post.Image = UploadImage(vm.FileImage);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notifyService.Success("Post created successfully!");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                return View();
            }

            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
                Image = post.Image 
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
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);
           
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ModifiedDate = DateTime.Now;
            post.ModifiedBy = loggedInUser!.FullName;
            post.Slug = SeoUrlHelper.FrientlyUrl(vm.Title!);
     
            if (vm.FileImage!= null)
            {
                post.Image = UploadImage(vm.FileImage);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Post updated successfully!");
            return RedirectToAction("Index", "post", new { area = "admin" });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Posts!.Find(id);
            if (item != null)
            {
                _context.Posts.Remove(item);
                _context.SaveChanges();
                _notifyService.Success("Post deleted successfully!");
                return Json(new { success = true });
            }
            _notifyService.Error("Post deleted failed!");
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
                _notifyService.Success("Posts deleted successfully!");
                return Json(new { success = true });
            }
            _notifyService.Error("Posts deleted failed!");
            return Json(new { success = false });
        }

        private string UploadImage(IFormFile file)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/post");
            string extension = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string uniqueFileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
