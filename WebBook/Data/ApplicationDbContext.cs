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
        public DbSet<Menu>? Menus { get; set; }
        public DbSet<Event>? Events { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<News>? News { get; set; }
        public DbSet<SystemSetting>? SystemSettings { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Supplier>? Suppliers{get; set;}
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Contact>? Contact { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetails { get; set; }
        public DbSet<Subscribe>? Subscribes { get; set; }

    }
}
