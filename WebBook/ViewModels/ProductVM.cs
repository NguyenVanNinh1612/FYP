using System.ComponentModel.DataAnnotations;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class ProductVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public string? Avatar { get; set; }
        public int NumberOfPage { get; set; }
        [Required]
        [StringLength(255)]
        public string Author { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        //public decimal PriceSale { get; set; }
        public int Quantity { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHome { get; set; }
        public bool IsHot { get; set; }
        public bool IsSale { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }

        public string? CategorySlug { get; set; }
       
    }
}
