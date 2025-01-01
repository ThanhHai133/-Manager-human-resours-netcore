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
using Microsoft.AspNetCore.Authorization;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class LeaveRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/LeaveRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaveRequests.Include(l => l.Employees);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/LeaveRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employees)
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // GET: Admin/LeaveRequests/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestVM viewmodel)
        {
            // Kiểm tra tính hợp lệ của các thuộc tính bắt buộc
            if (ModelState.IsValid) {
                var leaverequest = new LeaveRequest
                {
                    EmployeeId = viewmodel.EmployeeId,
                    StartDate = viewmodel.StartDate,
                    EndDate = viewmodel.EndDate,
                    Reason = viewmodel.Reason,
                    IsApproved = viewmodel.IsApproved,
                };
              
                _context.Add(leaverequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode", viewmodel.EmployeeId);
            return View(viewmodel);

        }

        // GET: Admin/LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        // POST: Admin/LeaveRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LeaveRequestVM viewmodel)
        {
            if (id != viewmodel.LeaveRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy bản ghi hiện có từ cơ sở dữ liệu
                    var leaverequest = await _context.LeaveRequests.FindAsync(id);
                    if (leaverequest == null)
                    {
                        return NotFound();
                    }

                    leaverequest.EmployeeId = viewmodel.EmployeeId;
                    leaverequest.StartDate = viewmodel.StartDate;
                    leaverequest.EndDate = viewmodel.EndDate;
                    leaverequest.Reason = viewmodel.Reason;
                    leaverequest.IsApproved = viewmodel.IsApproved;

                    _context.Update(leaverequest);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(viewmodel.LeaveRequestId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeCode", viewmodel.EmployeeId);
            return View(viewmodel);
        }

        // GET: Admin/LeaveRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employees)
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: Admin/LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestExists(Guid id)
        {
            return _context.LeaveRequests.Any(e => e.LeaveRequestId == id);
        }
    }
}
