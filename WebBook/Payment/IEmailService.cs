namespace WebBook.Payment
{
    public interface IEmailService
    {
        bool Send(string name, string subject, string content, string toMail);
    }
}
