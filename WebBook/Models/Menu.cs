using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Menu : CommonAbstract
    {
        public Menu()
        {
            News = new HashSet<News>();
            Posts= new HashSet<Post>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }
        public int Position { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
