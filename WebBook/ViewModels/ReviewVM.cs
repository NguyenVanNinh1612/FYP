namespace WebBook.ViewModels
{
    public class ReviewVM
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public byte Rating { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
