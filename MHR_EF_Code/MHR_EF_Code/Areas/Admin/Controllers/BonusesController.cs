using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BonusesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BonusesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Bonuses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bonus.Include(b => b.Employees);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Bonuses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonus = await _context.Bonus
                .Include(b => b.Employees)
                .FirstOrDefaultAsync(m => m.BonusId == id);
            if (bonus == null)
            {
                return NotFound();
            }

            return View(bonus);
        }

        // GET: Admin/Bonuses/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode");
            return View();
        }

        // POST: Admin/Bonuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BonusVM viewmodel)
        {
            if (ModelState.IsValid)
            {
                var bonus = new Bonus
                {
                    BonusName = viewmodel.BonusName,
                    Description = viewmodel.Description,
                    Amount = viewmodel.Amount,
                    EmployeeId = viewmodel.EmployeeId,
                };
                _context.Add(bonus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode", viewmodel.EmployeeId);
            return View(viewmodel);
        }

        // GET: Admin/Bonuses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonus = await _context.Bonus.FindAsync(id);
            if (bonus == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode", bonus.EmployeeId);
            return View(bonus);
        }

        // POST: Admin/Bonuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BonusId,BonusName,Description,Amount,EmployeeId")] Bonus bonus)
        {
            if (id != bonus.BonusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bonus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BonusExists(bonus.BonusId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode", bonus.EmployeeId);
            return View(bonus);
        }

        // GET: Admin/Bonuses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonus = await _context.Bonus
                .Include(b => b.Employees)
                .FirstOrDefaultAsync(m => m.BonusId == id);
            if (bonus == null)
            {
                return NotFound();
            }

            return View(bonus);
        }

        // POST: Admin/Bonuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bonus = await _context.Bonus.FindAsync(id);
            if (bonus != null)
            {
                _context.Bonus.Remove(bonus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BonusExists(Guid id)
        {
            return _context.Bonus.Any(e => e.BonusId == id);
        }
    }
}
