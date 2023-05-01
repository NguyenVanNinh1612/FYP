using Microsoft.AspNetCore.Identity;

namespace WebBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }

        public ICollection<Menu>? Menus { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Post>? Posts { get; set; }

    }
}
