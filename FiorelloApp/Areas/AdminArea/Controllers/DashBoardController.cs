using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin")]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
