using FiorelloApp.Areas.AdminArea.ViewModels.Product;
using FiorelloApp.Data;
using FiorelloApp.Extensions;
using FiorelloApp.Helpers;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly FiorelloDbContext _context;

        public ProductController(FiorelloDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View();

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            if (!ModelState.IsValid) return View();
            var files = productCreateVM.Photos;
            if (files.Length == 0)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(productCreateVM);
            }
            Product newProduct = new();
            //List<ProductImage> images = new();
            foreach (var file in files)
            {

                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(productCreateVM);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(productCreateVM);
                }
                ProductImage productImage = new();
                productImage.ImageUrl = await file.SaveFile();
                productImage.ProductId = newProduct.Id;
                if (files[0] == file)
                {
                    productImage.IsMain = true;
                }
                newProduct.ProductImages.Add(productImage);
            }
            //newProduct.ProductImages = images;
            newProduct.Name = productCreateVM.Name;
            newProduct.Price = productCreateVM.Price;
            newProduct.CategoryId = productCreateVM.CategoryId;
            newProduct.Count = productCreateVM.Count;
            newProduct.CreatedDate = DateTime.Now;
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            ProductDetailVM productDetailVM = new();
            productDetailVM.Name = product.Name;
            productDetailVM.Price = product.Price;
            productDetailVM.Count = product.Count;
            productDetailVM.CategoryName = product.Category.Name;
            productDetailVM.ProductImages = product.ProductImages;
            productDetailVM.CreatedDate = product.CreatedDate;
            return View(productDetailVM);
        }
        public async Task<IActionResult> SetMainPhoto(int? id)
        {
            if (id == null) return BadRequest();
            var image = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);
            if (image == null) return NotFound();
            image.IsMain = true;
            var mainImage = await _context.ProductImages.FirstOrDefaultAsync(p => p.IsMain && p.ProductId == image.ProductId);
            mainImage.IsMain = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = image.ProductId });
        }
        public async Task<IActionResult> DeleteProductPhoto(int? id)
        {
            if (id == null) return BadRequest();
            var image = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);
            if (image == null) return NotFound();
            Helper.DeleteImage(image.ImageUrl);
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction("Update", new { id = image.ProductId });
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();

            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            ProductUpdateVM productUpdateVM = new();
            productUpdateVM.Name = product.Name;
            productUpdateVM.Price = product.Price;
            productUpdateVM.Count = product.Count;
            productUpdateVM.CategoryId = product.CategoryId;
            productUpdateVM.Images = product.ProductImages;
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            return View(productUpdateVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateVM productUpdateVM)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            if (id == null) return BadRequest();

            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            productUpdateVM.Images = product.ProductImages;
            if (!ModelState.IsValid)
            {
                return View(productUpdateVM);
            }
            var files = productUpdateVM.Photos;
            if (files != null)
            {
                foreach (var file in files)
                {

                    if (!file.CheckContentType())
                    {
                        ModelState.AddModelError("Photos", "Duzgun file secim edin");
                        return View(productUpdateVM);
                    }
                    if (file.CheckSize(500))
                    {
                        ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                        return View(productUpdateVM);
                    }
                    ProductImage productImage = new();
                    productImage.ImageUrl = await file.SaveFile();
                    productImage.ProductId = product.Id;
                    productImage.IsMain = false;
                    product.ProductImages.Add(productImage);
                }
            }

            product.Name = productUpdateVM.Name;
            product.Price = productUpdateVM.Price;
            product.Count = productUpdateVM.Count;
            product.CategoryId = productUpdateVM.CategoryId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            var product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            foreach (var image in product.ProductImages)
            {
                Helper.DeleteImage(image.ImageUrl);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
