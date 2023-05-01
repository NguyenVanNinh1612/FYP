using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBook.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public byte Rating { get; set; }
        public string ApplicationUserId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ApplicationUser? ApplicationUser { get; set; }
        public Product? Product { get; set; }



       
       
    }

}
