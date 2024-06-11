using FiorelloApp.Models;

namespace FiorelloApp.ViewModels
{
    public class HomeVm
    {
        public IEnumerable<Slider> sliders { get; set; }
        public SliderContent SliderContent { get; set; }
    }
}
