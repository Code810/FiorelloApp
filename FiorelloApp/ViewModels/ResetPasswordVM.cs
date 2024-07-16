using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.ViewModels
{
    public class ResetPasswordVM
    {
        [MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(100), DataType(DataType.Password), Compare(nameof(Password))]
        public string RePassword { get; set; }

    }
}
