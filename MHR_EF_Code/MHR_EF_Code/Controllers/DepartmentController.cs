using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var department = await _context.department.ToListAsync();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(departmentVM viewModel )
        {
            var department = new department
            {
                Name = viewModel.Name,
                location = viewModel.location
            };
            if(ModelState.IsValid)
            {
               await _context.department.AddAsync(department);
               await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var d = await _context.department.FindAsync(id);
            if(d == null)
            {
                return NotFound();
            }
            return View(d);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(departmentVM viewModel)
        {
            var d = await _context.department.FindAsync(viewModel.DepartmentID);
            if(d == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                d.Name = viewModel.Name;
                d.location = viewModel.location;

                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(d);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var d = await _context.department.FirstOrDefaultAsync(row => row.DepartmentID == id);
            if (d != null)
            {
                 _context.department.Remove(d);
                 _context.SaveChanges();

                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }
    }
}
