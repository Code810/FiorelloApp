using FiorelloApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Product
{
    public class ProductDetailVM
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
