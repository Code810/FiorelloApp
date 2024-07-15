using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.ViewModels
{
    public class RegisterVM
    {
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(100), DataType(DataType.Password), Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
