using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var t = await _context.training.ToListAsync(); 
            return View(t);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var t = await _context.training.FindAsync(id);
            return View(t);
        }
        [HttpGet]
        public IActionResult create() 
        {   
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> create(TrainingVM viewModel)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            var t = new Training
            {
                TrainingName = viewModel.TrainingName,
                Description = viewModel.Description,
                Quantity = viewModel.Quantity,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                IsActive = viewModel.IsActive,
            };
#pragma warning restore CS8601 // Possible null reference assignment.
            if (viewModel.IsActive) 
            { 
                await _context.training.AddAsync(t);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(t);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var t = await _context.training.FindAsync(id);
            return View(t);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TrainingVM viewModel)
        {
            var t = _context.training.Find(viewModel.TrainingID);
            if(ModelState.IsValid)
            {
                t.TrainingName = viewModel.TrainingName;
                t.Description = viewModel.Description;
                t.Quantity = viewModel.Quantity;
                t.StartDate = viewModel.StartDate;
                t.EndDate = viewModel.EndDate;
                t.IsActive = viewModel.IsActive;

                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(t);
        }
        public async Task<IActionResult> Delete (Guid id)
        {
            var t = _context?.training.FirstOrDefaultAsync(row => row.TrainingID == id);
            await _context.training.Remove(t);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
