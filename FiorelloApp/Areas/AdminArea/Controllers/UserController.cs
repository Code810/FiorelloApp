using FiorelloApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? searchText)
        {
            var users = searchText == null ? await _userManager.Users.ToListAsync()
                : await _userManager.Users.Where(u => u.UserName.ToLower().Contains(searchText.ToLower()) ||
                u.FullName.ToLower().Contains(searchText.ToLower())).ToListAsync();

            return View(users);

        }
        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (id == null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}
