using FiorelloApp.Data;
using FiorelloApp.Models;
using FiorelloApp.Services;
using FiorelloApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<FiorelloDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<FiorelloDbContext>().AddDefaultTokenProviders();
        }
    }
}
