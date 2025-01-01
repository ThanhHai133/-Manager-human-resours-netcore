using System.Diagnostics;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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
