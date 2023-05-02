using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Supplier : Common
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}
