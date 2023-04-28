using Microsoft.AspNetCore.Mvc;
using WebBook.Data;

namespace WebBook.ViewComponents
{
    public class CartCheckoutViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<CartItem> carts)
        {
            return View(carts);
        }
    }
}
