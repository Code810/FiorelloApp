using FiorelloApp.Areas.AdminArea.ViewModels.Category;
using FiorelloApp.Data;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly FiorelloDbContext _context;

        public CategoryController(FiorelloDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid) return View(category);
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower())
)
            {
                ModelState.AddModelError("Name", "Bu adli category artiq movcuddur");
                return View(category);
            }
            var newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description,
                CreatedDate = DateTime.Now,
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
