using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Models;
using WebBook.Utilites;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public INotyfService _notifyService { get; }
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INotyfService notifyService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notifyService = notifyService;
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

    }
}
