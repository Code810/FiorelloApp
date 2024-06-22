﻿using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly FiorelloDbContext _fiorellaDbContext;

        public BlogController(FiorelloDbContext fiorellaDbContext)
        {
            _fiorellaDbContext = fiorellaDbContext;
        }

        public IActionResult Index()
        {
            var query = _fiorellaDbContext.Blogs.AsQueryable();
            ViewBag.BlogCount = query.Count();
            var datas = query.OrderByDescending(b => b.Id).Take(3).AsNoTracking().ToList();
            return View(datas);
        }


        public IActionResult Detail(int? id)
        {
            if (id is null) return NotFound();
            var blog = _fiorellaDbContext.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (blog is null) return NotFound();
            return View(blog);
        }

        public IActionResult LoadMore(int offset = 3)
        {
            var datas = _fiorellaDbContext.Blogs.Skip(offset).Take(3).ToList();
            return PartialView("_BlogPartialView", datas);
        }
    }
}
