using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Category : Common
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        ///public string? SeoTitle { get; set; }
        //public string? SeoDescription { get; set; }
        //public string? SeoKeywords { get; set; }
       
        public ICollection<Product>? Products { get; set; }
    }
}
