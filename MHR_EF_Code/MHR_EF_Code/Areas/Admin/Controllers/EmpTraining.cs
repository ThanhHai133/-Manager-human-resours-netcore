using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class EmpTraining : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpTraining(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empT = _context.EmployeeTraining.ToListAsync();
            return View(empT);
        }
        [HttpGet]
        public async Task<IActionResult> detail(Guid id)
        {
            var empT = await _context.EmployeeTraining.FindAsync(id);
            return View(empT);
        }
        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmpTraining viewModel)
        {
            var empT = new EmployeeTraining
            {

            };
            return View(viewModel);
        }
    }
}
