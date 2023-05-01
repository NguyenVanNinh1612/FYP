using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? FullName { get; set; }
        public string? Address { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Phone { get; set; }

        public bool IsSuper { get; set; }
    }
}
