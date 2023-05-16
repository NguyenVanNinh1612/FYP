using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class ResetPasswordVM
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string? NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessage = "Mật khẩu không khớp")]
        [Required]
        public string? ConfirmPassword { get; set; }

        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}
