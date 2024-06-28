using FiorelloApp.Data;
using FiorelloApp.Services.Interfaces;
using FiorelloApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Services
{
    public class BasketService : IBasketService
    {
        private readonly FiorelloDbContext _context;

        private readonly IHttpContextAccessor _contextAccessor;

        public BasketService(IHttpContextAccessor contextAccessor, FiorelloDbContext context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }
        public List<BasketVM> GetBasketList()
        {
            var list = GetBasketCookies();
            foreach (var item in list)
            {
                var existProduct = _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == item.Id);
                item.Name = existProduct.Name;
                item.Image = existProduct.ProductImages.FirstOrDefault(p => p.IsMain).ImageUrl;
            }
            return list;
        }

        public List<BasketVM> GetBasketCookies()
        {
            List<BasketVM> list = new();
            string basket = _contextAccessor.HttpContext.Request.Cookies["basket"];
            if (basket != null)
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            return list;
        }
    }
}
