using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService { get; }

        public ProductController(ApplicationDbContext context, INotyfService notifyService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notifyService = notifyService;
            _webHostEnvironment = webHostEnvironment;
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

            IEnumerable<Product> products = _context.Products!.OrderByDescending(x => x.Id);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }
          
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            //ViewBag.CategoryList = new SelectList(_context.Categories.ToLlist(), "Id", "Name");
            ViewBag.CategoryList = _context.Categories.ToList().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.SupplierList = new SelectList(_context.Suppliers.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name!);
                _context.Products!.Add(model);
                _context.SaveChanges();
                _notifyService.Success("Product created successfully!");

                return RedirectToAction("Index");
            }

            _notifyService.Error("Product created failed!");
            return View(model);

        }


        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "upload/images/product");
            string extension = Path.GetExtension(file.FileName);
            uniqueFileName = file.FileName + DateTime.Now.ToString("yymmssff") + extension;
            //uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }


    }
}
