using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.Data;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Identity;
using Intuit.Ipp.Data;

namespace MHR_EF_Code.Models.Entities
{
    public class LeaveRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LeaveRequestsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LeaveRequests
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUserAsync(User);
            if(users == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == users.Id);
            if (employee == null) { return RedirectToAction("Login", "Account"); }

            var leaveRequests = await _context.LeaveRequests.Where(lr => lr.EmployeeId == employee.EmployeeId).Include(lr => lr.Employees).ToListAsync(); 

            return View(leaveRequests);
        }

        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequestVM = await _context.LeaveRequests
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);
            if (leaveRequestVM == null)
            {
                return NotFound();
            }

            return View(leaveRequestVM);
        }

        // GET: LeaveRequests/Create
        public IActionResult Create()
        {
            return View();
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestVM leaveRequestVM)
        {
            var users = await _userManager.GetUserAsync(User);
            if (users == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == users.Id);
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var lr = new LeaveRequest
                {
                    EmployeeId = employee.EmployeeId,
                    StartDate = leaveRequestVM.StartDate,
                    EndDate = leaveRequestVM.EndDate,
                    Reason = leaveRequestVM.Reason,
                    IsApproved = false 
                };
                _context.LeaveRequests.Add(lr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequestVM);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequestVM = await _context.LeaveRequestVM.FindAsync(id);
            if (leaveRequestVM == null)
            {
                return NotFound();
            }
            return View(leaveRequestVM);
        }

        // POST: LeaveRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LeaveRequestId,EmployeeId,StartDate,EndDate,Reason,IsApproved")] LeaveRequestVM leaveRequestVM)
        {
            if (id != leaveRequestVM.LeaveRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveRequestVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestVMExists(leaveRequestVM.LeaveRequestId))
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
            return View(leaveRequestVM);
        }

        // GET: LeaveRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequestVM = await _context.LeaveRequestVM
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);
            if (leaveRequestVM == null)
            {
                return NotFound();
            }

            return View(leaveRequestVM);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var leaveRequestVM = await _context.LeaveRequestVM.FindAsync(id);
            if (leaveRequestVM != null)
            {
                _context.LeaveRequestVM.Remove(leaveRequestVM);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestVMExists(Guid id)
        {
            return _context.LeaveRequestVM.Any(e => e.LeaveRequestId == id);
        }
    }
}
