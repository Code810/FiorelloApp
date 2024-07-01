using FiorelloApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Product
{
    public class ProductUpdateVM
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }
        public IFormFile[]? Photos { get; set; }
        public List<ProductImage>? Images { get; set; }
    }
}
