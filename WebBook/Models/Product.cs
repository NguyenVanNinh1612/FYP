using System.ComponentModel.DataAnnotations;


namespace WebBook.Models
{
    public class Product : Common
    {
        public Product()
        {
            ProductImage = new HashSet<ProductImage>();
            OrderDetail = new HashSet<OrderDetail>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public string? Slug { get; set; }
        //public string ProductCode { get; set; }
        public string Description { get; set; }
        //public string Detail { get; set; }
        //public string? Avatar { get; set; }
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
        //public string? SeoTitle { get; set; }
        //public string? SeoDescription { get; set; }
        //public string? SeoKeywords { get; set; }


        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<ProductImage>? ProductImage { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetail { get; set; }
        public ICollection<Review>? Reviews { get; set; }

    }
}
