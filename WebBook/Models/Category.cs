using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Category : CommonAbstract
    {
        public Category()
        {
            this.News = new HashSet<News>();
            this.Posts= new HashSet<Post>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }
        public int Position { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
