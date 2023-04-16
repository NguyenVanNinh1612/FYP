using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }     
        public bool IsAvatar { get; set; }
        public virtual Product Product { get; set; }
    }
}
