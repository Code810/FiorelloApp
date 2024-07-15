using FiorelloApp.Data;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Controllers
{
    public class BasketController : Controller
    {
        private readonly FiorelloDbContext _fiorellaDbContext;


        public BasketController(FiorelloDbContext fiorellaDbContext)
        {
            _fiorellaDbContext = fiorellaDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBasket(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = _fiorellaDbContext.Products.FirstOrDefault(p => p.Id == id);
            if (existProduct == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if (basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            var existProductBasket = list.FirstOrDefault(p => p.Id == existProduct.Id);
            if (existProductBasket == null)
                list.Add(new BasketVM() { Id = existProduct.Id, BasketCount = 1 });
            else
                existProductBasket.BasketCount++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowBasket()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if (basket is null) list = new();

            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in list)
                {
                    var existProduct = _fiorellaDbContext.Products
                        .Include(p => p.ProductImages)
                        .FirstOrDefault(p => p.Id == item.Id);
                    item.Name = existProduct.Name;
                    item.Image = existProduct.ProductImages.FirstOrDefault(p => p.IsMain).ImageUrl;
                }
            }
            return View(list);
        }

    }
}
