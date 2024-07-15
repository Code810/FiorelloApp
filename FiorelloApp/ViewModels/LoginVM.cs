using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.ViewModels
{
    public class LoginVM
    {
        [MaxLength(100)]
        public string UserNameOrEmail { get; set; }
        [MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
