using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;

namespace WebBook.Repositories.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public Category CategoryGetById(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            return category;
        }

        public IList<Category> GetAll()
        {

            var query = from b in _context.Categories
                        select b;

            return query.ToList();
        }

       
    }
}
