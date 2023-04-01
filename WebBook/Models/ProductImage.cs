using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Image { get; set; }
        public bool IsDefault { get; set; }
    }
}
