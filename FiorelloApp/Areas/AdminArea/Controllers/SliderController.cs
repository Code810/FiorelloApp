using FiorelloApp.Areas.AdminArea.ViewModels.Slider;
using FiorelloApp.Data;
using FiorelloApp.Extensions;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {

        private readonly FiorelloDbContext _context;

        public SliderController(FiorelloDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.AsNoTracking().ToListAsync();
            return View(sliders);
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(p => p.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM sliderCreateVM)
        {
            var file = sliderCreateVM.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(sliderCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun file secim edin");
                return View(sliderCreateVM);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "faylin olcusu 300kb-dan az olmalidir");
                return View(sliderCreateVM);
            }

            Slider slider = new();
            slider.ImageUrl = await file.SaveFile();
            slider.CreatedDate = DateTime.Now;
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return NotFound();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", slider.ImageUrl);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(new SliderUpdateVM { ImageUrl = slider.ImageUrl });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM sliderUpdateVM)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            var file = sliderUpdateVM.Photo;
            sliderUpdateVM.ImageUrl = slider.ImageUrl;

            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(sliderUpdateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Duzgun file secim edin");
                return View(sliderUpdateVM);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "faylin olcusu 300kb-dan az olmalidir");
                return View(sliderUpdateVM);
            }
            string fileName = await file.SaveFile();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", slider.ImageUrl);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            slider.ImageUrl = fileName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
