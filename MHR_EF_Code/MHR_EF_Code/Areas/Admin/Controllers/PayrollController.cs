using Intuit.Ipp.Data;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PayrollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManager<AppUser> _UserManager;

        public PayrollController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _UserManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payEmp = await _context.Payroll.Include(p => p.Employee).ToListAsync();
            return View(payEmp);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            // Tìm kiếm nhân viên thông qua PayrollId
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PayrollId == id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            // Tìm kiếm thông tin liên quan đến nhân viên
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.EmployeeId == employee.EmployeeId);
            var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId);
            var overtime = await _context.Overtime.FirstOrDefaultAsync(o => o.EmployeeId == employee.EmployeeId);
            var bonus = await _context.Bonus.FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId);

            decimal diligent = 0m;
            if (attendance?.TotalDaysWorked > 25)
            {
                diligent = 500000m;
            }

            // Tìm kiếm vai trò của nhân viên dựa trên UserId
            var user = await _UserManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == employee.EmployeeId);
            var roles = user != null ? await _UserManager.GetRolesAsync(user) : new List<string>();

            decimal positionBonus = 0m;
            if (roles.Contains("Manager"))
            {
                positionBonus = 1500000m;
            }
            else if (roles.Contains("Employee"))
            {
                positionBonus = 1000000m;
            }

            decimal employeeInsurance = (contact?.BaseSalary ?? 0) * 0.105m;

            // Tính toán tổng lương sau khi trừ BHXH và cộng các khoản thưởng
            var totalSalary = ((contact?.BaseSalary ?? 0) / 25) * (attendance?.TotalDaysWorked ?? 0) +
                              ((((contact?.BaseSalary ?? 0) / 25) / 8) * (overtime?.Hours ?? 0) * 2) +
                              (bonus?.Amount ?? 0) + diligent + positionBonus - employeeInsurance;

            var viewModel = new EmployeeSalaryVM
            {
                Employees = employee,
                Contact = contact,
                Attendance = attendance,
                Overtime = overtime,
                TotalSalary = (decimal)totalSalary
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Detail(EmployeeSalaryVM viewmodel, Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PayrollId == id);
            if (employee == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.EmployeeId == employee.EmployeeId);
            var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId);
            var overtime = await _context.Overtime.FirstOrDefaultAsync(o => o.EmployeeId == employee.EmployeeId);
            var payroll = await _context.Payroll.FirstOrDefaultAsync(p => p.PayrollId == id);
            var bonus = await _context.Bonus.FirstOrDefaultAsync(b => b.EmployeeId == employee.EmployeeId);

            attendance.TotalDaysWorked = viewmodel.Attendance.TotalDaysWorked;
            overtime.Hours = viewmodel.Overtime.Hours;

            decimal diligent = 0m;
            if (attendance.TotalDaysWorked > 25)
            {
                diligent = 500000m;
            }
            var user = await _UserManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == employee.EmployeeId);
            var roles = await _UserManager.GetRolesAsync(user);

            decimal positionBonus = 0m;
            if (roles.Contains("Manager"))
            {
                positionBonus = 1500000m;
            }
            else if (roles.Contains("Employee"))
            {
                positionBonus = 1000000m;
            }
            decimal employeeInsurance = contact.BaseSalary * 0.105m;


            var totalSalary = ((contact?.BaseSalary ?? 0) / 25) * (attendance?.TotalDaysWorked ?? 0) +
                              ((((contact?.BaseSalary ?? 0) / 25) / 8) * (overtime?.Hours ?? 0) * 2) +
                              (bonus?.Amount ?? 0) + diligent + positionBonus - employeeInsurance;

            payroll.TotalSalary = totalSalary;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Payroll");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var employee = await _context.Employees
         .Include(e => e.Contact)
         .Include(e => e.Attendance)
         .Include(e => e.Overtime)
         .Include(e => e.Department)
         .FirstOrDefaultAsync(e => e.PayrollId == id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }


            var viewModel = new EmployeeSalaryVM
            {
                Employees = employee,
                Contact = employee.Contact,
                Attendance = employee.Attendance,
                Overtime = employee.Overtime
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeSalaryVM viewmodel, Guid id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.PayrollId == id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }


            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId);

            var overtime = await _context.Overtime
                .FirstOrDefaultAsync(o => o.EmployeeId == employee.EmployeeId);

            var payroll = await _context.Payroll
                .FirstOrDefaultAsync(p => p.PayrollId == id);

            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            if (attendance != null)
            {
                attendance.TotalDaysWorked = viewmodel.Attendance.TotalDaysWorked;
                attendance.StartDate = viewmodel.Attendance.StartDate;
                attendance.EndDate = viewmodel.Attendance.EndDate;

                _context.Attendances.Update(attendance);
            }

            if (overtime != null)
            {
                overtime.Hours = viewmodel.Overtime.Hours;

                _context.Overtime.Update(overtime);
            }

            if (payroll != null)
            {
                payroll.TotalSalary = viewmodel.Payroll.TotalSalary;
                payroll.PayDate = viewmodel.Payroll.PayDate;

                _context.Payroll.Update(payroll);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return View(viewmodel); // Trả lại view nếu xảy ra lỗi
            }

            return RedirectToAction("Index", "Payroll");
        }

    }
}
