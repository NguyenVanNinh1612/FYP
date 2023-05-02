using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Menu : Common
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string? Slug { get; set; }
        [Required]
        public byte Position { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
