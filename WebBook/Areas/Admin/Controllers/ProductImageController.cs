using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Super, Admin")]
    public class ProductImageController : Controller
	{
		private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductImageController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

                return Json(new { success = true, imageId = productImage.Id});
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


        [HttpPost]
        public IActionResult SetAvatar(int id)
        {
            var productImage = _context.ProductImages.FirstOrDefault(x => x.Id == id);
            
            if (productImage != null)
            {
                var pimages = _context.ProductImages.Where(x => x.ProductId == productImage.ProductId).ToList();
                foreach(var item in pimages)
                {
                    if (item.Id == id)
                    {
                        item.IsAvatar = true;
                    }
                    else
                    {
                        item.IsAvatar = false;
                    }
                }
               

                _context.SaveChanges();

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
