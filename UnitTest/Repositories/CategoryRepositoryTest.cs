using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.ContentModel;
using NUnit.Framework;
using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;
using WebBook.Repositories.Repository;

namespace UnitTest.Repositories
{
    [TestFixture]
    public class CategoryRepositoryTest
    {
        private ICategoryRepository _repository;
        private Mock<ApplicationDbContext> _context;
        private Mock<DbSet<Category>> _dbSet;

        [SetUp]
        public void SetUp()
        {
            _dbSet = new Mock<DbSet<Category>>();
            var data = new List<Category>()
            {
                new Category() { Id = 1, Name="Tieu thuyet", Slug = "tieu-thuyet"},
                new Category(){Id = 2, Name = "Van hoc", Slug="van-hoc"},
                new Category(){Id = 3, Name = "Sach giao khoa", Slug = "sach-giao-khoa"}
            }.AsQueryable();

            _dbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _context = new Mock<ApplicationDbContext>();
            _context.Setup(x => x.Set<Category>()).Returns(_dbSet.Object);
            _repository = new CategoryRepository(_context.Object);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnListCategory()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }
    }
}
