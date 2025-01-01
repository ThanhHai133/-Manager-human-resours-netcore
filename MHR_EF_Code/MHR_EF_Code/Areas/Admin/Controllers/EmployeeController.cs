using Intuit.Ipp.Data;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EmployeeController> _logger;

        public ILogger<EmployeeController> Logger { get; }

        public EmployeeController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<EmployeeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int? page)
        {

            // Lấy tất cả danh sách nhân viên
            var employees = from e in _context.Employees.Include(e => e.Department)
                            select e;

            // Thực hiện tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.FullName.Contains(searchString) ||
                                                 e.EmployeeCode.Contains(searchString));
            }

            // Số mục mỗi trang
            int pageSize = 7;
            int pageNumber = (page ?? 1);

            // Tổng số mục
            int totalItems = await employees.CountAsync();

            // Thực hiện phân trang
            var paginatedEmployees = await employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Gán các giá trị vào ViewBag
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchString = searchString;

            return View(paginatedEmployees);
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Departments = new SelectList(_context.department, "DepartmentID", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(EmployeeVM viewModel)
        {
            if (viewModel != null)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var existingUser = await _userManager.FindByNameAsync(viewModel.username);
                        if (existingUser != null)
                        {
                            ModelState.AddModelError("username", "Username already exists");
                            return View(viewModel);
                        }

                        var user = new AppUser
                        {
                            UserName = viewModel.username,
                        };
                        var result = await _userManager.CreateAsync(user, viewModel.Password);

                        if (result.Succeeded)
                        {
                            if (!await _roleManager.RoleExistsAsync("Employee"))
                            {
                                await _roleManager.CreateAsync(new IdentityRole("Employee"));
                            }
                            await _userManager.AddToRoleAsync(user, "Employee");
                            var count = await _userManager.Users.CountAsync();
                            var employee = new Employees
                            {
                                EmployeeCode = "NV00" + (count + 1),
                                FullName = viewModel.FullName,
                                HireDate = viewModel.HireDate,
                                IdentityNumber = viewModel.Identity,
                                Education = viewModel.Education,
                                DepartmentID = viewModel.DepartmentID,
                                UserId1 = user.Id
                            };
                            //if (viewModel.Photo != null)
                            //{
                            //    var fileName = Path.GetFileName(viewModel.PhotoFile.FileName); 
                            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/employee", fileName);
                            //    using (var stream = new FileStream(filePath, FileMode.Create))
                            //    { await viewModel.PhotoFile.CopyToAsync(stream); 
                            //    } // Lưu đường dẫn ảnh vào đối tượng
                            //   employee.Photo = "/img/employee/" + fileName; 
                            //}

                                _context.Employees.Add(employee);
                            await _context.SaveChangesAsync(); // Lưu nhân viên trước

                            // Sau khi lưu nhân viên, gán EmployeeId cho các đối tượng phụ
                            var contact = new Contact
                            {
                                ContactType = viewModel.ContactType,
                                Description = viewModel.Description,
                                BaseSalary = viewModel.BaseSalary,
                                StartDate = viewModel.StartDate,
                                EndDate = viewModel.EndDate,
                                EmployeeId = employee.EmployeeId // Gán EmployeeId
                            };
                            _context.Contacts.Add(contact);

                            var attendence = new Attendance
                            {
                                TotalDaysWorked = 0,
                                StartDate = (DateTime)viewModel.StartDate,
                                EndDate = new DateTime(viewModel.StartDate.Value.Year,
                                    viewModel.StartDate.Value.Month,
                                    DateTime.DaysInMonth(viewModel.StartDate.Value.Year, viewModel.StartDate.Value.Month)),
                                EmployeeId = employee.EmployeeId // Gán EmployeeId
                            };
                            _context.Attendances.Add(attendence);

                            var overtime = new Overtime
                            {
                                Hours = 0,       
                                EmployeeId = employee.EmployeeId 
                            };
                            _context.Overtime.Add(overtime);

                            var payroll = new Payroll
                            {
                                EmployeeId = employee.EmployeeId 
                            };
                            _context.Payroll.Add(payroll);

                            await _context.SaveChangesAsync(); 

                            user.EmployeeId = employee.EmployeeId;
                            employee.PayrollId = payroll.PayrollId;
                            await _userManager.UpdateAsync(user);
                            
                            await transaction.CommitAsync();

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                           
                            await transaction.RollbackAsync();
                            await _userManager.DeleteAsync(user);  // Xóa người dùng
                            ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                            ViewBag.Departments = new SelectList(_context.department, "DepartmentID", "Name");
                            return View(viewModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception (Optional: Log the error message)
                        await transaction.RollbackAsync();
                        var user = await _userManager.FindByNameAsync(viewModel.username);
                        if (user != null)
                        {
                            await _userManager.DeleteAsync(user);
                        }
                        ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                        ViewBag.Departments = new SelectList(_context.department, "DepartmentID", "Name");
                        return View(viewModel);
                    }
                }
            }

            ViewBag.Departments = new SelectList(_context.department, "DepartmentID", "Name");
            return View(viewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var employee = await _context.Employees
               .Where(e => e.EmployeeId == id)
               .Include(e => e.User) // Assuming there is a navigation property named AppUser
               .FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound();
            }
            var detailViewModel = new EmployeeDetailVM
            {
                EmployeeCode = employee.EmployeeCode,
                FullName = employee.FullName,
                HireDate = employee.HireDate,
                Identity = employee.IdentityNumber,
                Education = employee.Education,
                DepartmentID = employee.DepartmentID,
                Username = employee.User?.UserName,
                Email = employee.User?.Email
            };

            return View(detailViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, string? userName)
        {
            // Lấy thông tin nhân viên từ cơ sở dữ liệu
            var employee = await _context.Employees
                .Where(e => e.EmployeeId == id)
                .Include(e => e.User)
                .Include(e=> e.Department)// Assuming there is a navigation property named AppUser
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            // Tạo ViewModel từ thông tin nhân viên
            var viewModel = new EmployeeVM
            {
                EmployeeCode = employee.EmployeeCode,
                FullName = employee.FullName,
                HireDate = employee.HireDate,
                Identity = employee.IdentityNumber,
                Education = employee.Education,
                Photo = employee.Photo,
                DepartmentID = employee.DepartmentID,
                username = employee.User?.UserName,
                Password = "", // Không lấy mật khẩu từ cơ sở dữ liệu
                ConfirmPassword = "" // Không lấy mật khẩu xác nhận từ cơ sở dữ liệu
            };
            ViewBag.Departments = new SelectList(await _context.department.ToListAsync(), "DepartmentID", "Name");
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EmployeeVM viewModel)
        {
            // Lấy thông tin nhân viên từ cơ sở dữ liệu
            var employee = await _context.Employees
                .Where(e => e.EmployeeId == id)
                .Include(e => e.User)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            // Kiểm tra xem username có tồn tại hay không
            var existingUser = await _userManager.FindByNameAsync(viewModel.username);
            if (existingUser != null && existingUser.Id != employee.User.Id)
            {
                ModelState.AddModelError("username", "Username already exists");
                return View(viewModel);
            }

            // Cập nhật thông tin user
            var user = employee.User;
            if (user != null)
            {
                user.UserName = viewModel.username;

                // Đổi mật khẩu nếu có thông tin hợp lệ
                if (!string.IsNullOrEmpty(viewModel.CurrentPassword) &&
                    !string.IsNullOrEmpty(viewModel.Password) &&
                    !string.IsNullOrEmpty(viewModel.ConfirmPassword))
                {
                    if (viewModel.Password == viewModel.ConfirmPassword)
                    {
                        var passwordChangeResult = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.Password);
                        if (!passwordChangeResult.Succeeded)
                        {
                            foreach (var error in passwordChangeResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match.");
                        return View(viewModel);
                    }
                }

                // Cập nhật user
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    foreach (var error in userUpdateResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(viewModel);
                }
            }

            // Cập nhật thông tin nhân viên
            employee.EmployeeCode = viewModel.EmployeeCode;
            employee.FullName = viewModel.FullName;
            employee.HireDate = viewModel.HireDate;
            employee.IdentityNumber = viewModel.Identity;
            employee.Education = viewModel.Education;
            employee.Photo = viewModel.Photo;
            employee.DepartmentID = viewModel.DepartmentID;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            ViewBag.Departments = new SelectList(await _context.department.ToListAsync(), "DepartmentID", "DepartmentName");
            return RedirectToAction("Index", "Employee", new { area = "Admin" });
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employees
        .Include(e => e.User)
        .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            if (employee.User != null)
            {
                employee.User.EmployeeId = null;
                _context.Users.Update(employee.User);
                await _context.SaveChangesAsync();
            }

            // Xóa Employee sau
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}