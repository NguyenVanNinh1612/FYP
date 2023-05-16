using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Event : Common
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public int Position { get; set; }
        public string? FlashSale { get; set; }
    }
}
