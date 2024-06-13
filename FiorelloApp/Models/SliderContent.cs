using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Models
{
    public class SliderContent : BaseEntity
    {
        [Required, StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public string SignImageUrl { get; set; }
    }
}
