
using WebBook.Models;

namespace WebBook.Repositories.IRepository
{
    public interface ICategoryRepository
    {
        IList<Category> GetAll();

        void Add(Category category);

        Category CategoryGetById(int id);
    }

   
}
