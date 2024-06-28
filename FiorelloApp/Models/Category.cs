using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Models
{
    public class Category : BaseEntity
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
