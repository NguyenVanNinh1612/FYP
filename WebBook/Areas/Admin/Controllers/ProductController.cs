using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using WebBook.Common;
using WebBook.Data;
using WebBook.Models;
using WebBook.ViewModels;
using X.PagedList;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Super, Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService { get; }

        public ProductController(ApplicationDbContext context, INotyfService notifyService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notifyService = notifyService;
            _webHostEnvironment = webHostEnvironment;
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

            var listProducts = new List<ProductVM>();

            var products = _context.Products!.OrderByDescending(x => x.Id).ToList();
            foreach (var item in products)
            {
                ProductVM vm = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Discount = item.Discount,
                   // PriceSale = item.PriceSale,
                    Quantity = item.Quantity,
                    SupplierId = item.SupplierId,
                    CategoryId = item.CategoryId,
                    CategoryName = _context?.Categories?.Find(item.CategoryId)?.Name,
                    SupplierName = _context?.Suppliers?.Find(item.SupplierId)?.Name,
                    Avatar = _context?.ProductImages?.Where(x => x.ProductId == item.Id).ToList()?.FirstOrDefault(x => x.IsAvatar)?.ImageName
                };

                listProducts.Add(vm);
            }
           
            if (!string.IsNullOrEmpty(searchString))
            {
                listProducts = listProducts.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).ToList();
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
        public async Task<IActionResult> Create(Product model, List<IFormFile> Files, string isDefault)
        {
            if (ModelState.IsValid)
            {
                if (Files != null && Files.Count > 0)
                {
                    foreach (var file in Files)
                    {
                        if (isDefault == file.FileName)
                        {
                            model.ProductImage!.Add(new ProductImage
                            {
                                ImageName = UploadImage(file),
                                ProductId = model.Id,
                                IsAvatar = true
                            });
                        }
                        else
                        {
                            model.ProductImage!.Add(new ProductImage
                            {
                                ImageName = UploadImage(file),
                                ProductId = model.Id,
                                IsAvatar = false
                            });
                        }
                    }

                }
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
                model.CreatedBy = loggedInUser?.FullName;
                model.ModifiedBy = loggedInUser?.FullName;

                model.Slug = SeoUrlHelper.FrientlyUrl(model.Name);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                await _context.Products!.AddAsync(model);
                await _context.SaveChangesAsync();

                _notifyService.Success("Product created successfully!");
                return RedirectToAction("Index", "product", new {area="admin"});
            }


            ViewBag.CategoryList = new SelectList(_context.Categories!.ToList(), "Id", "Name");
            ViewBag.SupplierList = new SelectList(_context.Suppliers!.ToList(), "Id", "Name");
            _notifyService.Error("Product created failed!");
            return View(model);

        }

        public ActionResult Edit(int id)
        {
            var product = _context.Products!.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return View();
            }
            ProductVM vm = new()
            {
                Id = product.Id,
                Name = product.Name,
                //ProductCode = product.ProductCode,
                Description = product.Description,
                //Detail = product.Detail,
                NumberOfPage = product.NumberOfPage,
                Author = product.Author,
                Price = product.Price,
                Discount = product.Discount,
                //PriceSale = product.PriceSale,
                Quantity = product.Quantity,
                IsFeature = product.IsFeature,
                IsHome = product.IsHome,
                IsHot = product.IsHot,
                IsSale = product.IsSale,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
               
                //SeoTitle = product.SeoTitle,
                //SeoDescription = product.SeoDescription,
                //SeoKeywords = product.SeoKeywords
            };

            ViewBag.CategoryList = new SelectList(_context.Categories!.ToList(), "Id", "Name");
            ViewBag.SupplierList = new SelectList(_context.Suppliers!.ToList(), "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM vm)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

                var product = _context.Products!.FirstOrDefault(x => x.Id == vm.Id);

                product!.Name = vm.Name;
                product.Slug = SeoUrlHelper.FrientlyUrl(product.Name);
               // product.ProductCode = vm.ProductCode;
                product.Description = vm.Description;
               // product.Detail = vm.Detail;
                product.NumberOfPage = vm.NumberOfPage;
                product.Author = vm.Author;
                product.Price = vm.Price;
                product.Discount = vm.Discount;
                //product.PriceSale = vm.PriceSale;
                product.Quantity = vm.Quantity;
                product.IsFeature = vm.IsFeature;
                product.IsHome = vm.IsHome;
                product.IsHot = vm.IsHot;
                product.IsSale = vm.IsSale;
                product.CategoryId = vm.CategoryId;
                product.SupplierId = vm.SupplierId;
                //product.SeoTitle = vm.SeoTitle;
                //product.SeoDescription = vm.SeoDescription;
                //product.SeoKeywords = vm.SeoKeywords;
                product.ModifiedDate = DateTime.Now;
                product.ModifiedBy = loggedInUser?.FullName;

                await _context.SaveChangesAsync();
                _notifyService.Success("Product updated successfully!");
                return RedirectToAction("Index", "product", new { area = "admin" });
            }

            _notifyService.Error("Product updated failed!");
            return View(vm);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products!.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                var productImage = _context.ProductImages.Where(x => x.ProductId == product.Id);
                foreach (var item in productImage)
                {
                    _context.ProductImages.Remove(item);
                    //string path = "~/uploads/images/product/" + item.ImageName;
                    //System.IO.File.Delete(path);
                }
                _context.Products!.Remove(product);
                _context.SaveChanges();
                _notifyService.Success("Product deleted successfully!");
                return Json(new { success = true });
            }
            _notifyService.Error("Product deleted failed!");
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
                        var productImage = _context.ProductImages.Where(x => x.ProductId == Convert.ToInt32(item));
                        foreach (var image in productImage)
                        {
                            _context.ProductImages.Remove(image);
                           // string path = "~/uploads/images/product/" + image.ImageName;
                           // System.IO.File.Delete(path);
                        }
                        var obj = _context.Products!.Find(Convert.ToInt32(item));
                        _context.Products.Remove(obj!);
                        _context.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }



        private string UploadImage(IFormFile file)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/product");
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
