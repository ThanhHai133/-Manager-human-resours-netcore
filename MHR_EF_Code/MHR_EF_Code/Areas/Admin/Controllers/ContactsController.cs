using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MHR_EF_Code.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Contacts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contacts.Include(c => c.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Contacts/Details/5
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
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.Include(c => c.Employee).FirstOrDefaultAsync(e => e.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Contact contact)
        {
            var Contact = await _context.Contacts
            .Include(e => e.Employee) // Chỉ include nếu cần
            .FirstOrDefaultAsync(c => c.ContactId == contact.ContactId);

            if (ModelState.IsValid) {
                Contact.ContactType = contact.ContactType;
                Contact.Description = contact.Description;
                Contact.BaseSalary = contact.BaseSalary;
                Contact.StartDate = contact.StartDate;
                Contact.EndDate = contact.EndDate;


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(contact);

        }



        private bool ContactExists(Guid id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
