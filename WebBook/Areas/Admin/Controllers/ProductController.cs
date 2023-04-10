using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using WebBook.ViewModels;
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

            //IEnumerable<Product> products = _context.Products!.OrderByDescending(x => x.Id);
            var listProducts = new List<ProductVM>();
            _context.Products!.OrderByDescending(x => x.Id).ToList().ForEach(product =>
            {
                listProducts.Add(new ProductVM(product.Id,
                                               product.Name,
                                               product.Avatar,
                                               product.Price,
                                               product.PriceSale,
                                               product.Quantity,
                                               product.CategoryId,
                                               product.SupplierId,
                                               categoryName: _context.Categories.Find(product.CategoryId).Name,
                                               supplierName: _context.Suppliers.Find(product.SupplierId).Name));
            });



            if (!string.IsNullOrEmpty(searchString))
            {
                listProducts = (List<ProductVM>)listProducts.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(listProducts.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_context.Categories!.ToList(), "Id", "Name");
            ViewBag.SupplierList = new SelectList(_context.Suppliers!.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model, List<IFormFile> Files, IFormFile Avatar)
        {
            if (ModelState.IsValid)
            {
                if (Files != null && Files.Count > 0)
                {
                   // model.Files = Files;
                    model.Files = Files.Select(x => x).ToList();
                   
                    model.AvatarFile = Avatar;
                    foreach (var file in Files)
                    {
                        model.ProductImage!.Add(new ProductImage
                        {
                            ImageName = UploadImage(file),
                            ProductId = model.Id
                        });
                    }

                }


                model.Avatar = UploadImage(Avatar);
                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name);
                _context.Products!.Add(model);
                _context.SaveChanges();

                _notifyService.Success("Product created successfully!");
                return RedirectToAction("Index");
            }


            ViewBag.CategoryList = new SelectList(_context.Categories!.ToList(), "Id", "Name");
            ViewBag.SupplierList = new SelectList(_context.Suppliers!.ToList(), "Id", "Name");
            _notifyService.Error("Product created failed!");
            return View(model);

        }



        public IActionResult Edit(int id)
        {
            var product = _context.Products!.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return View();
            }

            List<string> images = new List<string>();
            foreach (var item in _context.ProductImages.Where(x => x.ProductId == id).ToList())
            {
                images.Add(item.ImageName);
            }

            List<IFormFile>? files = product.Files;
           

            ProductVM productVM = new ProductVM(
                    product.Id,
                    product.Name,
                    product.ProductCode,
                    product.Description,
                    product.Detail,
                    product.Avatar,
                    product.NumberOfPage,
                    product.Author,
                    product.Price,
                    product.PriceSale,
                    product.Quantity,
                    product.IsFeature,
                    product.IsHome,
                    product.IsHot,
                    product.IsSale,
                    product.CategoryId,
                    product.SupplierId,
                    product.SeoTitle,
                    product.SeoDescription,
                    product.SeoKeywords,
                    listImages: images,
                    files,
                    product.AvatarFile

                );

            ViewBag.CategoryList = new SelectList(_context.Categories!.ToList(), "Id", "Name");
            ViewBag.SupplierList = new SelectList(_context.Suppliers!.ToList(), "Id", "Name");
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM vm, List<IFormFile> Files, IFormFile Avatar)
        {
            if (ModelState.IsValid)
            {

                _notifyService.Success("Product updated successfully!");
                return RedirectToAction("Index");
            }

            _notifyService.Error("Product updated failed!");
            return View(vm);
        }




        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/product");
            string extension = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            uniqueFileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }


    }
}
