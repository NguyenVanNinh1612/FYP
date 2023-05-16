using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;

namespace WebBook.Repositories.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Product> GetProductByCategory(int id)
        {
            var products = _context.Products.Where(x => x.CategoryId == id).ToList();
            return products;
        }
    }
}
