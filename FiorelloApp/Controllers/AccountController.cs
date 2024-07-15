using FiorelloApp.Helpers;
using FiorelloApp.Models;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace FiorelloApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser user = new()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(user, RolesEnum.member.ToString());
            //send mail
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token = token },
                Request.Scheme, Request.Host.ToString());

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("nadirssh@code.edu.az", "Email Confirm Fiorella");
            mailMessage.To.Add(new MailAddress(user.Email));
            mailMessage.Subject = "Verify Email";
            string body = $"<a href=`{link}`>Confirm email</a>";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            using (StreamReader streamReader = new StreamReader("wwwroot/emailTemplate/htmlpage.html"))
            {
                body = streamReader.ReadToEnd();
            };
            SmtpClient smtpClient = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("nadirssh@code.edu.az", "ezts zpby grqu rvhp"),
            };
            smtpClient.Send(mailMessage);



            return Content("user created...");
        }
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) return NotFound();
            await _userManager.ConfirmEmailAsync(appUser, token);
            //await _signInManager.SignInAsync(appUser,true);
            return RedirectToAction("index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "(username or email) or password is wrong...");
                    return View(loginVM);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "user is lockout...");
                return View(loginVM);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "go to verify email");
                return View(loginVM);
            }
            if (user.IsBlocked)
            {
                ModelState.AddModelError("", "user is blocked...");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "(username or email) or password is wrong...");
                return View(loginVM);
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin")) return RedirectToAction("Index", "dashboard", new { area = "adminarea" });
            if (ReturnUrl == null)
                return RedirectToAction("index", "home");
            return Redirect(ReturnUrl);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> AddRole()
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            if (!await _roleManager.RoleExistsAsync("member"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "member" });
            if (!await _roleManager.RoleExistsAsync("superadmin"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "superadmin" });
            return Content("roles added");
        }
    }
}
