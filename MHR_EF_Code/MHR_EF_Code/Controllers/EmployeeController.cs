using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _Context;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            _Context = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var e = await _Context.Employees.Include(e => e.Department).ToListAsync();
            return View(e);
        }
        [HttpGet]
        public IActionResult create() {
            ViewBag.Departments = new SelectList(_Context.department, "DepartmentID", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> create(EmployeeVM viewModel)
        {
            var e = new Employee
            {
                DepartmentID = viewModel.DepartmentID,
                FullName = viewModel.FullName,
                EmployeeCode = viewModel.EmployeeCode,
                HireDate = viewModel.HireDate,
                Identity = viewModel.Identity,
                Education = viewModel.Education,
                Photo = viewModel.Photo,
            };
            if (ModelState.IsValid) { 
            await _Context.Employees.AddAsync(e);
            await _Context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
