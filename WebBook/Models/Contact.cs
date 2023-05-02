using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Contact : Common
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        //public string Website { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
