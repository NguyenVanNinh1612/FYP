using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;

namespace WebBook.Repositories.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> GetAll()
        {
            return _context.Reviews.ToList();
        }
    }
}
