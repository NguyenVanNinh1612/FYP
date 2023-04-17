
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductImageController : Controller
	{
		private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductImageController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int id)
		{
			var productImages = _context.ProductImages.Where(x => x.ProductId == id).ToList();
			return View(productImages);
		}


        [HttpPost]
        public IActionResult Add(IFormFile file, int productId)
        {
            if (file != null)
            {
                ProductImage productImage = new()
                {
                    ProductId = productId,
                    ImageName = UploadImage(file),
                    IsAvatar = false
                };
                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
                return Json(new { success = true, imageId = productImage.Id });
            }


            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var productImage = _context.ProductImages.FirstOrDefault(x=>x.Id == id);
            if(productImage != null)
            {
                _context.ProductImages.Remove(productImage);
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });

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
