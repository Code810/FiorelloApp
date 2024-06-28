using FiorelloApp.Data;
using FiorelloApp.Services;
using FiorelloApp.Services.Interfaces;
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
        }
    }
}
