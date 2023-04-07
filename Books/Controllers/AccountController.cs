using Books.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Books.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<UserModel> _usermanager;
        private readonly SignInManager<UserModel> _signInManager;
        public readonly DbContext _context;

        public AccountController(DbContext context, UserManager<UserModel> usermanager, SignInManager<UserModel> signInManager)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public IActionResult Login()
        {
            

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel userLogInData)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogInData);
            }

            await _signInManager.PasswordSignInAsync(userLogInData.UserName, userLogInData.Password, false, false);

            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["isRegistered"] = false;
            ViewData["isPasswordLenght"] = true;


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel userRegisterData)
        {
            ViewData["isPasswordLenght"] = true;
            ViewData["isRegistered"] = false;


            if (!ModelState.IsValid)
            {
                return View(userRegisterData);
            }
            if (_context.Users.Any(x => x.UserName.Equals(userRegisterData.UserName)))
            {
                ViewData["isRegistered"] = true;

                return View(userRegisterData);
            }
            if (userRegisterData.Password.Length < 5)
            {
                ViewData["isPasswordLenght"] = false;
                return View(userRegisterData);

            }




            var newUser = new UserModel
            {
                UserName = userRegisterData.UserName,
                FirstName = userRegisterData.FirstName,
                LastName = userRegisterData.LastName,
                Email = userRegisterData.Email,

                DateOfBirth = userRegisterData.DateOfBirth


            };

            await _usermanager.CreateAsync(newUser, userRegisterData.Password);



            return RedirectToAction("Login", "Account");
        }
    }
}
