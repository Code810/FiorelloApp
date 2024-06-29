using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
