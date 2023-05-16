using WebBook.Models;

namespace WebBook.Repositories.IRepository
{
    public interface IReviewRepository
    {
        IList<Review> GetAll();
    }
}
