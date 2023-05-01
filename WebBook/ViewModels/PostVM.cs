namespace WebBook.ViewModels
{
    public class PostVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }   
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ?Image { get; set; }
    }
}
