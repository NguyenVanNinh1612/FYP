using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set;} = DateTime.Now;
    }

}
