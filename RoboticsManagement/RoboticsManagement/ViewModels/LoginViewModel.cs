using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password does't match.")]
        public string ConfirmPassword { get; set; }
    }
}
