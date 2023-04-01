using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBook.Models;

namespace WebBook.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser>? ApplicationUser { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Adv>? Advs { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<News>? News { get; set; }
        public DbSet<SystemSetting>? SystemSettings { get; set; }
        public DbSet<ProductCategory>? ProductCategories { get; set; }
        public DbSet<Product>? Prodcts { get; set; }
        public DbSet<Contact>? Contact { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails { get; set; }
        public DbSet<Subscribe>? Subscribes { get; set; }

    }
}
