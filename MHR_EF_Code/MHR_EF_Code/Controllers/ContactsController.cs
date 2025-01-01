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

namespace MHR_EF_Code.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.UserId1 == user.Id);
            var applicationDbContext = _context.Contacts.Where(c=> c.EmployeeId == emp.EmployeeId).Include(c => c.Employee);
            return View(applicationDbContext);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
      
    }
}
