using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Product : CommonAbstract
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? ProductCode { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public int Quantity { get; set; }
        public bool IsFearure { get; set; }
        public bool IsHome { get; set; }
        public bool IsHot { get; set; }
        public int ProductCategoryId { get; set; }
        public bool IsSale { get; set; }
        public int CategoryId { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }

        public virtual ProductCategory? ProductCategory { get; set; }
    }
}
