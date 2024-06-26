
using FiorelloApp.Data;
using Microsoft.AspNetCore.Mvc;

public class BlogViewComponent : ViewComponent
{
    private readonly FiorelloDbContext _context;

    public BlogViewComponent(FiorelloDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int take = 3)
    {
        var blogs = _context.Blogs.Take(take).ToList();
        return View(await Task.FromResult(blogs));
    }

}
