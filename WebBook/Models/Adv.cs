using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Adv : CommonAbstract
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Type { get; set; }
    }
}
