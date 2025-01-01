using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Identity;
using MHR_EF_Code.ViewModels;
using Intuit.Ipp.Data;

namespace MHR_EF_Code.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TrainingsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == user.Id);
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var joinedTrainings = await _context.EmployeeTraining
                                                .Where(et => et.EmployeeId == employee.EmployeeId)
                                                .Select(et => et.TrainingId)
                                                .ToListAsync();
            var trainings = _context.training.AsQueryable();

            // Số mục mỗi trang
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var totalCount = await trainings.CountAsync();
            var paginatedTrainings = await trainings.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.JoinedTrainings = joinedTrainings;

            var viewModel = new TrainingListVM
            {
                Trainings = paginatedTrainings,
                JoinedTrainings = joinedTrainings
            };

            return View(viewModel);
        }




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

        [HttpPost]
        public async Task<IActionResult> Join(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == user.Id);
                    if (employee != null)
                    {
                        // Kiểm tra xem nhân viên đã tham gia khóa đào tạo chưa
                        var existingEntry = await _context.EmployeeTraining
                                                          .FirstOrDefaultAsync(et => et.EmployeeId == employee.EmployeeId && et.TrainingId == id);

                        if (existingEntry == null)
                        {
                            var training = await _context.training.FirstOrDefaultAsync(t => t.TrainingID == id);
                            if (training != null && training.Quantity > 0)
                            {
                                var employeeTraining = new EmployeeTraining
                                {
                                    EmployeeId = employee.EmployeeId,
                                    TrainingId = id
                                };
                                _context.EmployeeTraining.Add(employeeTraining);

                                // Giảm số lượng `Quantity` của khóa đào tạo
                                training.Quantity -= 1;

                                await _context.SaveChangesAsync();

                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unjoin(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == user.Id);
                    if (employee != null)
                    {
                        var employeeTraining = await _context.EmployeeTraining
                                                             .FirstOrDefaultAsync(et => et.EmployeeId == employee.EmployeeId && et.TrainingId == id);
                        if (employeeTraining != null)
                        {
                            _context.EmployeeTraining.Remove(employeeTraining);

                            var training = await _context.training.FirstOrDefaultAsync(t => t.TrainingID == id);
                            if (training != null)
                            {
                                training.Quantity += 1;
                                await _context.SaveChangesAsync();

                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index");
        }
    }
    }
