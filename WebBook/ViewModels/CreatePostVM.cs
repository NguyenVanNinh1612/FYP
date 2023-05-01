using System.ComponentModel.DataAnnotations;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class CreatePostVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? FileImage { get; set; }
    }
}
