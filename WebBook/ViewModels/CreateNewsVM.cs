
using System.ComponentModel.DataAnnotations;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class CreateNewsVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public int CategoryId { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }

        public virtual Menu? Menu { get; set; }
    }
}
