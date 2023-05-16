using Microsoft.AspNetCore.Identity;
using WebBook.Data;
using WebBook.Models;

namespace WebBook.Utilites
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(WebsiteRoles.Super).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Super)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Customer)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser()
                {
                    FullName = "Nguyễn Văn Ninh",
                    Email = "admin@gmail.com",
                    UserName = "Ninh",
                    Address = "Thái Nguyên"
                }, "Admin@0011").Wait();

                var appUser = _context.ApplicationUser!.FirstOrDefault(x => x.Email == "admin@gmail.com");
                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.Super).GetAwaiter().GetResult();  
                }
                _context.SaveChanges();


            }
            if(_context.Events.Count() == 0)
            {
                Event e = new()
                {
                    Name = "Flash Sale",
                    FlashSale = "0.0.0"
                };
                _context.Events.Add(e);
                _context.SaveChanges();
            }
        }
    }
}
