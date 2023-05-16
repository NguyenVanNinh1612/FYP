using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebBook.Data;
using WebBook.Models;
using WebBook.Payment;
using WebBook.Utilites;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public INotyfService _notifyService { get; }
        public AccountController(UserManager<ApplicationUser> userManager, INotyfService notifyService,
            ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _context = context;
            _signInManager = signInManager;
            _emailService = emailService;
       
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
        public IActionResult Login(string? returnUrl)
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                ViewBag.returnUrl = returnUrl;
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "Home");

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == vm.Username);
            if (existingUser == null)
            {
                _notifyService.Error("Username không tồn tại");
                ViewBag.returnUrl = returnUrl;
                return View(vm);
            }
            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if (!verifyPassword)
            {
                _notifyService.Error("Mật khẩu không chính xác");
                ViewBag.returnUrl = returnUrl;
                return View(vm);
            }
            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);

            _notifyService.Success("Đăng nhập thành công!");
            var roles = await _userManager.GetRolesAsync(existingUser);
            if (roles[0] == "Admin" || roles[0] == "Super")
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

          
            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }
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


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user == null)
                {
                    _notifyService.Error("Email không chính xác");
                    return View(vm);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                //var callbackUrl = Url.Page(
                //    "/Account/ResetPassword",
                //    pageHandler: null,
                //    values: new { code },
                //    protocol: Request.Scheme
                //    );

                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, protocol:Request.Scheme);

                _emailService.Send("VNBOOK", "Đặt lại mật khẩu", $"Để đặt lại mật khẩu hãy <a href='{callbackUrl}'>Bấm vào đây</a>.", vm.Email);

                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(vm);
        }

        public IActionResult ResetPassword(string? code)
        {
            if (code == null)
            {
                return BadRequest("Mã Token không tồn tại");
            }
            else
            {
                ResetPasswordVM vm = new()
                {
                    Code = code
                };

                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(vm);
            //}

            //Tim user theo email
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotEmailConfirmation");
            }
            //Dat lai password cho user, co kiem tra ma token khi doi
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, vm.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordSuccess");
            }
            
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(vm);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }
    }
}
