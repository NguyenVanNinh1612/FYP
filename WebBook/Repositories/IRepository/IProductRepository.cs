using WebBook.Models;

namespace WebBook.Repositories.IRepository
{
    public interface IProductRepository
    {
        IList<Product> GetProductByCategory(int id);
    }
}
