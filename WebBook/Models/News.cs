using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class News : CommonAbstract
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }

      
       
    }
}
