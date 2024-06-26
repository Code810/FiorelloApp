using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
