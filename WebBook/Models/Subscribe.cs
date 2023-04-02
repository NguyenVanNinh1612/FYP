using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Subscribe
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
