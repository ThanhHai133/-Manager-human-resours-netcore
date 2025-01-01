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

    }
}
