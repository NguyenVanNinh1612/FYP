using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public class Order : CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>(); 
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public bool PaymentMethod { get; set; }
        public int Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
