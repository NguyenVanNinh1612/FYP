using Microsoft.AspNetCore.Mvc;
using WebBook.Data;
using WebBook.ViewModels;

namespace WebBook.ViewComponents
{
    public class ReviewViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ReviewViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int productId)
        {
            var reviews = _context.Reviews?.OrderByDescending(x => x.CreatedDate).Where(x=>x.ProductId == productId).ToList();
            var listReviewVM = reviews?.Select(x => new ReviewVM()
            {
                Id = x.Id,
                Content = x.Content,
                Rating = x.Rating,
                FullName = _context.ApplicationUser.FirstOrDefault(u => u.Id == x.ApplicationUserId).FullName,
                CreatedDate = x.CreatedDate

            }).ToList();

            ViewBag.countReview = listReviewVM.Count;
            return View(listReviewVM);
        }
    }
}
