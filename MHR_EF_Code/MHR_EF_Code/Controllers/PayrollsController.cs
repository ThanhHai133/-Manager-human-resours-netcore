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
    public class PayrollsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PayrollsController(ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Payrolls
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == user.Id);
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var pay = await _context.Payroll
                                                .Where(et => et.EmployeeId == employee.EmployeeId)
                                                .ToListAsync();
            return View(pay);
        }

        // GET: Payrolls/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollId == id);
            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        // GET: Payrolls/Create

    }
}
