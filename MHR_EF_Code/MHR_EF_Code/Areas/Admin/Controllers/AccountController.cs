using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.username };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false); if (result.Succeeded)
                    {
                        Console.WriteLine("User authenticated and signed in.");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Failed to sign in user.");
                    }
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var getAllAccount = await _userManager.Users
                .Include(e => e.Employee) 
                .ToListAsync();

            return View(getAllAccount);
        }

    }
}
