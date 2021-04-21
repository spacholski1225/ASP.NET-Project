using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class EmployeeRegistrationViewModel //todO extend about emplyee informations
    {
        [Required(ErrorMessage="User name is required")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="Password and Confirm Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
