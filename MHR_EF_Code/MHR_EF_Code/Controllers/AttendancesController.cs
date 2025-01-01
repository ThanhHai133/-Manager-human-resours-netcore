using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace MHR_EF_Code.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AttendancesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public UserManager<AppUser> UserManager { get; }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == user.Id);
            var attendanceList = await _context.Attendances.Where(a => a.EmployeeId == emp.EmployeeId).ToListAsync();
            return View(attendanceList);
        }

    }
}
