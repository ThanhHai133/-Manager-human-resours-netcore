using System.Diagnostics;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userRole = Request.Cookies["UserRole"]; 
            if (userRole == null) 
            { 
                return RedirectToAction("Login", "Account"); 
            }
            if (userRole == "Admin")
            { 
                return RedirectToAction("Index", "Home", new { area = "Admin" }); 
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.UserId = user?.Id; ViewBag.UserName = user?.UserName ?? "Guest";
            }
            else
            {
                ViewBag.UserId = null;
                ViewBag.UserName = "Guest";
            }
            ViewBag.GetToTalUsers = _context.Employees.Count();
            ViewBag.GetTraining = _context.training.Count();
            ViewBag.GetBonuses = _context.Bonus.Count();
            ViewBag.GetLeaveRequest = _context.LeaveRequests.Count();
            return View();
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
