using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }
        public IFormFile[] Photos { get; set; }
    }
}
