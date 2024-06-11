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
                SliderContent = _fiorellaDbContext.SliderContents.SingleOrDefault(),

            };
            return View(homeVm);
        }
    }
}