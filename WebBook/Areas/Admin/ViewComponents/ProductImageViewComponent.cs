using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.Areas.Admin.ViewComponents
{
    [Area("Admin")]
    public class ProductImageViewComponent : ViewComponent  
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductImageViewComponent(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IViewComponentResult Invoke(int productId)
        {
            var productImages = _context.ProductImages.Where(x => x.ProductId == productId).ToList();
            return View(productImages);
        }

        [HttpPost]
        public IActionResult AddImage(IFormFile file, int productId)
        {
            if (file != null)
            {
                ProductImage productImage = new ProductImage();
                productImage.ProductId = productId;
                productImage.ImageName = UploadImage(file);
                productImage.IsAvatar = false;
                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
            }
            return (IActionResult)View("Default");
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
