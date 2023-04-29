using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class OrderVM
    {
        [Required(ErrorMessage = "Tên người nhận không được để trống!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email không được để trống!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Tỉnh/Thành phố không được để trống!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Quận/Huyện trấn không được để trống!")]
        public string District { get; set; }
        [Required(ErrorMessage = "Phường/Xã không được để trống!")]
        public string Ward { get; set; }

        public bool PaymentMethod { get; set; }
    }
}
