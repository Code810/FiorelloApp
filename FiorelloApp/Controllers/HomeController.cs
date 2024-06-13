using FiorelloApp.Data;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FiorelloDbContext _fiorellaDbContext;

        public HomeController(FiorelloDbContext fiorellaDbContext)
        {
            _fiorellaDbContext = fiorellaDbContext;
        }

        public IActionResult Index()
        {
            var homeVm = new HomeVm()
            {
                sliders = _fiorellaDbContext.Sliders
                .AsNoTracking()
                .ToList(),
                SliderContent = _fiorellaDbContext.SliderContents.AsNoTracking().SingleOrDefault(),
                categories = _fiorellaDbContext.Categories.AsNoTracking().ToList(),
                products = _fiorellaDbContext.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .AsNoTracking().ToList(),
                Banner = _fiorellaDbContext.Banners.AsNoTracking().SingleOrDefault(),
                BannerContents = _fiorellaDbContext.BannerContents.AsNoTracking().ToList(),
                Experts = _fiorellaDbContext.Experts.AsNoTracking().ToList(),

            };
            return View(homeVm);
        }
    }
}