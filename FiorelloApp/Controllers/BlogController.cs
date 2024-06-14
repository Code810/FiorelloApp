using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly FiorelloDbContext _fiorellaDbContext;

        public BlogController(FiorelloDbContext fiorellaDbContext)
        {
            _fiorellaDbContext = fiorellaDbContext;
        }

        public IActionResult Index()
        {
            return View(_fiorellaDbContext.Blogs.OrderByDescending(b => b.Id).AsNoTracking().ToList());
        }

        public IActionResult Detail(int? id)
        {
            if (id is null) return NotFound();
            var blog = _fiorellaDbContext.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (blog is null) return NotFound();
            return View(blog);
        }
    }
}
