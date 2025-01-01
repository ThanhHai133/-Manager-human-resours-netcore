using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using Intuit.Ipp.Data;
using MHR_EF_Code.ViewModels;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Trainings
        public async Task<IActionResult> Index()
        {
            return View(await _context.training.ToListAsync());
        }

        // GET: Admin/Trainings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training
                .FirstOrDefaultAsync(m => m.TrainingID == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingName,Description,Quantity,StartDate,EndDate,IsActive")] TrainingVM trainingVM)
        {
            if (ModelState.IsValid)
            {
 
                var training = new Training
                {
                    TrainingName = trainingVM.TrainingName,
                    Description = trainingVM.Description,
                    Quantity = trainingVM.Quantity,
                    StartDate = trainingVM.StartDate,
                    EndDate = trainingVM.EndDate,
                    IsActive = trainingVM.IsActive
                };

                _context.training.Add(training); // Thêm đối tượng Training vào cơ sở dữ liệu
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang Index
            }
            return View(trainingVM); // Trả về view với dữ liệu ViewModel nếu ModelState không hợp lệ
        }

        // GET: Admin/Trainings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }

        // POST: Admin/Trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TrainingID,TrainingName,Description,Quantity,StartDate,EndDate,IsActive")] Training training)
        {
            if (id != training.TrainingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.TrainingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        // GET: Admin/Trainings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training
                .FirstOrDefaultAsync(m => m.TrainingID == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Admin/Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var training = await _context.training.FindAsync(id);
            if (training != null)
            {
                _context.training.Remove(training);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(Guid id)
        {
            return _context.training.Any(e => e.TrainingID == id);
        }
        [HttpGet]
        public async Task<IActionResult> join()
        {
            var getList = await _context.EmployeeTraining
                .Include(j => j.Employees)
                .Include(j => j.Training).ToListAsync();
            if (getList == null || !getList.Any())
            { Console.WriteLine("No entries found."); }

            return View(getList);
        }
    }

}
