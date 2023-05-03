using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using WebBook.Data;
using WebBook.Models;
using WebBook.Utilites;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }
        public AccountController(UserManager<ApplicationUser> userManager, INotyfService notifyService, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkUserByEmail != null)
            {
                _notifyService.Error("Email đã tồn tại");
                return View(vm);
            }
            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);
            if (checkUserByUsername != null)
            {
                _notifyService.Error("Username đã tồn tại");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                FullName = vm.FullName,
                UserName = vm.UserName,
                Address = vm.Address,
                PhoneNumber = vm.Phone
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.Customer);

                _notifyService.Success("Đăng ký tài khoản thành công!");
                return View();
            }
            return View(vm);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "Home");

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == vm.Username);
            if (existingUser == null)
            {
                _notifyService.Error("Username không tồn tại");
                return View(vm);
            }
            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if (!verifyPassword)
            {
                _notifyService.Error("Mật khẩu không chính xác");
                return View(vm);
            }
            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);

            _notifyService.Success("Đăng nhập thành công!");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notifyService.Success("Đăng xuất thành công!");
            return RedirectToAction("Index", "Home");
        }


        public IActionResult YourOrder(string userName)
        {
            var checkUser = _userManager.Users.FirstOrDefault(u => u.UserName == userName);
            if (checkUser == null)
            {
                return View();
            }

            var order = _context.Orders.OrderByDescending(x=>x.Id).Where(x => x.CreatedBy == checkUser.UserName).ToList();

            return View(order);
        }

        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders?.FirstOrDefault(x => x.Id == id);
            if (order != null)
            {
                var orderDetails = _context.OrderDetails?.Where(x => x.OrderId == order.Id).ToList();
            var carts = new List<CartItem>();
            foreach (var item in orderDetails)
            {
                CartItem cart = new()
                {
                    ProductId = item.ProductId,
                    ProductName = _context.Products.FirstOrDefault(x => x.Id == item.ProductId).Name,
                    ProductImage = _context.ProductImages.FirstOrDefault(x => x.ProductId == item.ProductId && x.IsAvatar).ImageName,
                    Price = (decimal)(_context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price - (_context.Products.FirstOrDefault(x => x.Id == item.ProductId).Price * (decimal)0.01
                    * _context.Products?.FirstOrDefault(x => x.Id == item.ProductId).Discount)),
                    Quantity = item.Quantity
                };
                carts.Add(cart);
            }

            ViewBag.carts = carts;

            }
            return View(order);
        }

    }
}
