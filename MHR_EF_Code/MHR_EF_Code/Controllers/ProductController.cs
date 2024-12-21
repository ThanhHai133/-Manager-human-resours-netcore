using MHR_EF_Code.Data;
using MHR_EF_Code.Models.Entities;
using MHR_EF_Code.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var product = await dbContext.product.ToListAsync();
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await dbContext.product.FindAsync(id);
            return View(product);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(productVM viewmodel)
        {
            var product = new products
            {
                Name = viewmodel.Name,
                Description = viewmodel.Description,
            };
            if (ModelState.IsValid)
            {
                await dbContext.product.AddAsync(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(viewmodel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
            var product = await dbContext.product.FindAsync(id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(productVM viewmodel)
        {
            var product = await dbContext.product.FindAsync(viewmodel.id);
            if (ModelState.IsValid)
            {
                product.Name = viewmodel.Name;
                product.Description = viewmodel.Description;

                await dbContext.SaveChangesAsync();

                return RedirectToAction("index");
            }  
          
            return View(viewmodel);
        }
        public async Task<IActionResult>Delete(Guid id)
        {
            var product = await dbContext.product.FirstOrDefaultAsync(row => row.id == id);
            if (product != null)
            {
                dbContext.product.Remove(product);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
