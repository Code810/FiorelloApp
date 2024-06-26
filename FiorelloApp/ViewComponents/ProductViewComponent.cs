using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly FiorelloDbContext _context;

        public ProductViewComponent(FiorelloDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take = 10)
        {
            var products = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Take(take).ToList();
            return View(await Task.FromResult(products));
        }
    }
}
