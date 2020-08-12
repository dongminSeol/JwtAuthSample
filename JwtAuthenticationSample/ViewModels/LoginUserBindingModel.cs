using System.ComponentModel.DataAnnotations;

namespace JwtAuthenticationSample.ViewModels
{
    public class LoginUserBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } 
    }
}
