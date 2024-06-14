using FiorelloApp.Models;

namespace FiorelloApp.ViewModels
{
    public class HomeVm
    {
        public IEnumerable<Slider> sliders { get; set; }
        public SliderContent SliderContent { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Product> products { get; set; }
        public Banner Banner { get; set; }
        public IEnumerable<BannerContent> BannerContents { get; set; }
        public IEnumerable<Expert> Experts { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }


    }
}
