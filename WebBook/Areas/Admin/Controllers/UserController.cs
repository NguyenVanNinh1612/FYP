using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBook.Models;
using WebBook.Utilites;
using WebBook.ViewModels;

namespace WebBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public INotyfService _notifyService { get; }
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,INotyfService notifyService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notifyService= notifyService;
        }

        [Authorize(Roles = "Super")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var vm = users.Select(x => new UserVM()
            {
                Id = x.Id,
                FullName = x.FullName,
                UserName = x.UserName,
                Email = x.Email
            }).ToList();

            foreach (var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                user.Role = role.FirstOrDefault();
            }

            return View(vm);
        }

        [Authorize(Roles = "Super")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                _notifyService.Error("User does not exitst");
                return View();
            }
            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName
            };
            return View(vm);
        }

        [Authorize(Roles = "Super")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if (existingUser == null)
            {
                _notifyService.Error("User does not exists");
                return View(vm);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);
            if (result.Succeeded)
            {
                _notifyService.Success("Password reset successfully!");
                return RedirectToAction(nameof(Index));
            }
            return View(vm);

        }

        [Authorize(Roles = "Super")]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }


        [Authorize(Roles = "Super")]
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
                _notifyService.Error("Email already exists");
                return View(vm);
            }
            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);
            if (checkUserByUsername != null)
            {
                _notifyService.Error("Username already exists");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                FullName = vm.FullName,
                UserName = vm.UserName,
                Address = vm.Address
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if (result.Succeeded)
            {
                if (vm.IsSuper)
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.Super);

                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.Admin);

                }

                _notifyService.Success("User registered successfully!");
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            return View(vm);
        }

        //[HttpGet("Admin/Login")]
        //public IActionResult Login()
        //{
        //    if (!HttpContext.User.Identity!.IsAuthenticated)
        //    {
        //        return View(new LoginVM());
        //    }
        //    return RedirectToAction("Index", "Home", new { area = "Admin" });

        //}


        //[HttpPost("Admin/login")]
        //public async Task<IActionResult> Login(LoginVM vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(vm);
        //    }
        //    var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == vm.Username);
        //    if (existingUser == null)
        //    {
        //        _notifyService.Error("Username does not exist");
        //        return View(vm);
        //    }
        //    var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
        //    if (!verifyPassword)
        //    {
        //        _notifyService.Error("Password does not match");
        //        return View(vm);
        //    }
        //    await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);
            
        //    _notifyService.Success("Login successfully!");
        //    return RedirectToAction("Index", "Home", new { area = "Admin" });
        //}

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            //return RedirectToAction("Index", "Home", new { area = "Admin" });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("AccessDenied")]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
