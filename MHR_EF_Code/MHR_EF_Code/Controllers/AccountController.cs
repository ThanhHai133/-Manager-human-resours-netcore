using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ApplicationDbContext _context { get; }

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                            var userRole = await _userManager.IsInRoleAsync(user, "Admin") ? "Admin" : "User";
                            Response.Cookies.Append("UserRole", userRole, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTimeOffset.UtcNow.AddDays(1)
                            });

                            if (await _userManager.IsInRoleAsync(user, "Admin"))
                            {

                                return RedirectToAction("Index", "Home", new { area = "Admin" });

                            }
                            else
                            {

                                return RedirectToAction("Index", "Home");
                            }

                        }
                }
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            return View(model);
        }

        [HttpPost] 
        public async Task<IActionResult> Logout() 
        {
            Response.Cookies.Delete("UserRole");
            await _signInManager.SignOutAsync(); 
            return RedirectToAction("Index", "Home"); 
        }
        public async Task<IActionResult> Index()
        {
            var getAllAccount = await _userManager.Users.ToListAsync();
            return View(getAllAccount);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            string idString = id.ToString(); 
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == idString);

            if (employee == null)
            {
                return NotFound();
            }

            // Chuyển đổi entity sang ViewModel
            var employeeVM = new Employees
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                FullName = employee.FullName,
                HireDate = employee.HireDate,
                IdentityNumber = employee.IdentityNumber,
                Education = employee.Education,
                Photo = employee.Photo,
                Position = employee.Position,
            };

            return View(employee); 
        }
    }
}
