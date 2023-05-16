using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
