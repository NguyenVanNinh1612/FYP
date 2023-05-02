using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Post:Common
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
