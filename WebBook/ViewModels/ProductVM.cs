using System.ComponentModel.DataAnnotations;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class ProductVM
    {


        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string? Avatar { get; set; }
        public int NumberOfPage { get; set; }
        [Required]
        [StringLength(255)]
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public int Quantity { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHome { get; set; }
        public bool IsHot { get; set; }
        public bool IsSale { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }
        
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }

        public List<string>? ListImages { get; set; }
       


        public ProductVM(int id, string name, string productCode, string description, string detail,
            string? avatar, int numberOfPage, string author, decimal price, decimal priceSale, 
            int quantity, bool isFeature, bool isHome, bool isHot, bool isSale, int categoryId, 
            int supplierId, string? seoTitle, string? seoDescription, string? seoKeywords,  List<string> listImages)
        {
            Id = id;
            Name = name;
            ProductCode = productCode;
            Description = description;
            Detail = detail;
            Avatar = avatar;
            NumberOfPage = numberOfPage;
            Author = author;
            Price = price;
            PriceSale = priceSale;
            Quantity = quantity;
            IsFeature = isFeature;
            IsHome = isHome;
            IsHot = isHot;
            IsSale = isSale;
            CategoryId = categoryId;
            SupplierId = supplierId;
            SeoTitle = seoTitle;
            SeoDescription = seoDescription;
            SeoKeywords = seoKeywords;
          
            ListImages = listImages;
          
        }

        public ProductVM(int id, string name, string? avatar, decimal price, decimal priceSale, int quantity, int categoryId, int supplierId, string categoryName, string supplierName)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
            Price = price;
            PriceSale = priceSale;
            Quantity = quantity;
            CategoryId = categoryId;
            SupplierId = supplierId;
            CategoryName = categoryName;
            SupplierName = supplierName;
        }

        

        public ProductVM(int id, string name, string? avatar, decimal price, decimal priceSale, int quantity, int categoryId, int supplierId, List<string> ListImages)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
            Price = price;
            PriceSale = priceSale;
            Quantity = quantity;
            CategoryId = categoryId;
            SupplierId = supplierId;
            this.ListImages = ListImages;
        }

        public ProductVM()
        {
        }
    }
}
