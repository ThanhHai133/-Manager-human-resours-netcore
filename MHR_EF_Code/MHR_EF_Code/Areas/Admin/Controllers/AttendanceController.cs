using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AttendanceController    
        public async Task<ActionResult> Index(string searchString, int? page)
        {
            // Lấy dữ liệu từ bảng Attendances và bao gồm Employee
            var queryableAttendance = _context.Attendances.Include(a => a.Employee).AsQueryable();

            // Nếu có chuỗi tìm kiếm, áp dụng bộ lọc
            if (!string.IsNullOrEmpty(searchString))
            {
                queryableAttendance = queryableAttendance.Where(a =>
                    a.Employee.FullName.Contains(searchString) ||
                    a.Employee.EmployeeCode.Contains(searchString));
            }

            // Tổng số bản ghi sau khi tìm kiếm
            int totalRecords = await queryableAttendance.CountAsync();

            // Số bản ghi trên mỗi trang
            int pageSize = 7;
            int pageNumber = page ?? 1;

            // Thực hiện phân trang
            var paginatedAttendance = await queryableAttendance
                .OrderBy(a => a.Employee.FullName) // Sắp xếp để đảm bảo dữ liệu hiển thị ổn định
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gán các giá trị vào ViewBag
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SearchString = searchString;

            return View(paginatedAttendance);
        }



        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var attendance = await _context.Attendances
                                      .Include(a => a.Employee) 
                                      .ThenInclude(a=> a.Department)
                                      .FirstOrDefaultAsync(a => a.AttendanceId == id);

            return View(attendance);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Attendance modelAT)
        {
            var attendace = await _context.Attendances.FindAsync(modelAT.AttendanceId);
            if(attendace == null)
            {
                return NotFound();
            }
            if (modelAT != null)
            {
                attendace.TotalDaysWorked = modelAT.TotalDaysWorked;
                attendace.StartDate = modelAT.StartDate;
                attendace.EndDate = modelAT.EndDate;


                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(modelAT);
        }
    }
}
