using WebBook.Models;

namespace WebBook.Payment
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Order model, HttpContext context);
        Order PaymentExecute(IQueryCollection collections);
    }
}
