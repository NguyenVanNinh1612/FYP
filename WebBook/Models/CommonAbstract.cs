namespace WebBook.Models
{
    public class CommonAbstract
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string? ModifiedBy { get; set; }
    }
}
