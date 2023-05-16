using Microsoft.EntityFrameworkCore;
using Moq;
using WebBook.Data;
using WebBook.Models;
using WebBook.Repositories.IRepository;
using WebBook.Repositories.Repository;

namespace UnitTests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTest
    {
        private IProductRepository _repository;
        private Mock<ApplicationDbContext> _context;
        private Mock<DbSet<Product>> _dbSet;


        [SetUp]
        public void SetUp()
        {

            var data = new List<Product>()
            {
                new Product() 
                { 
                    Id = 1,
                    Name="Ngu Van",
                    Slug = "ngu-van",
                    Description = "sach ngu van",
                    NumberOfPage = 20,
                    Author = "nxb Giao duc",
                    Price = 200000,
                    Discount = 15,
                    Quantity = 20,
                    CategoryId = 1,
                    SupplierId = 1,
                    CreatedBy="abc",
                    ModifiedBy="abc",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                 new Product()
                {
                    Id = 2,
                    Name="Toan",
                    Slug = "toan",
                    Description = "sach ngu van",
                    NumberOfPage = 20,
                    Author = "nxb Giao duc",
                    Price = 200000,
                    Discount = 15,
                    Quantity = 20,
                    CategoryId = 1,
                    SupplierId = 2,
                    CreatedBy="abc",
                    ModifiedBy="abc",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                  new Product()
                {
                    Id = 1,
                    Name="Vat ly",
                    Slug = "vat-ly",
                    Description = "sach ngu van",
                    NumberOfPage = 20,
                    Author = "nxb Giao duc",
                    Price = 200000,
                    Discount = 15,
                    Quantity = 20,
                    CategoryId = 2,
                    SupplierId = 1,
                    CreatedBy="abc",
                    ModifiedBy="abc",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },

            }.AsQueryable();
            _dbSet = new Mock<DbSet<Product>>();
            _dbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());



            _context = new Mock<ApplicationDbContext>();
            _context.Setup(x => x.Products).Returns(_dbSet.Object);
            _repository = new ProductRepository(_context.Object);
        }

        [Test]
        public void GetProduct_WhenCalled_ReturnListProductByCategory()
        {
            var result = _repository.GetProductByCategory(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}
