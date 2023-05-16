using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;
using WebBook.Repositories.Repository;

namespace UnitTests.Repositories
{
    public class ReviewRepositoryTest
    {
        private IReviewRepository _repository;
        private Mock<ApplicationDbContext> _context;
        private Mock<DbSet<Review>> _dbSet;


        [SetUp]
        public void SetUp()
        {
            _dbSet = new Mock<DbSet<Review>>();
          
            var data = new List<Review>()
            {
                new Review{Id = 1, Rating = 3, Content = "abc", ProductId=1, ApplicationUserId = "2"},
                new Review{Id = 2, Rating = 3, Content = "abc", ProductId=1, ApplicationUserId = "2"},
            }.AsQueryable();

            _dbSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());



            _context = new Mock<ApplicationDbContext>();
            _context.Setup(x => x.Reviews).Returns(_dbSet.Object);
            _repository = new ReviewRepository(_context.Object);

        }

        [Test]
        public void GetAll_WhenCalled_ReturnListReview()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}
