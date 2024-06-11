
using FiorelloApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Data
{
    public class FiorelloDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }
        public FiorelloDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
