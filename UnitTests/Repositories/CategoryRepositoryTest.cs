using Microsoft.EntityFrameworkCore;
using Moq;
using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;
using WebBook.Repositories.Repository;

namespace UnitTests.Repositories
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
            var data = new List<Category>()
            {
                new Category() { Id = 1, Name="Tieu thuyet", Slug = "tieu-thuyet", CreatedBy="abc", ModifiedBy="abc",
                    CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now},
                new Category() { Id = 2, Name = "Van hoc", Slug="van-hoc", CreatedBy="abc", ModifiedBy="abc", 
                    CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now},
                new Category() { Id = 3, Name = "Sach giao khoa", Slug = "sach-giao-khoa", CreatedBy="abc", ModifiedBy="abc",
                    CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now}
            }.AsQueryable();
            _dbSet = new Mock<DbSet<Category>>();
            _dbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _context = new Mock<ApplicationDbContext>();
            _context.Setup(x => x.Categories).Returns(_dbSet.Object);
            _repository = new CategoryRepository(_context.Object);
        }


        [Test]
        public void GetAll_WhenCalled_ReturnListCategory()
        {
            var result = _repository.GetAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void CreateCategory_saves_a_category_via_context()
        {
            var mockSet = new Mock<DbSet<Category>>();

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var service = new CategoryRepository(mockContext.Object);
            service.Add(new Category() 
            {   Id = 1, Name = "Tieu thuyet", Slug = "tieu-thuyet",
                CreatedBy = "abc", ModifiedBy = "abc",
                CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now 
            });

            mockSet.Verify(m => m.Add(It.IsAny<Category>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
