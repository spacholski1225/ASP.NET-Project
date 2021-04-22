using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class CompanyRegistartionViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(50, ErrorMessage = "Too long email")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Invalid NIP number."), MinLength(10)]
        public string NIP { get; set; }
        [Required]
        [MaxLength(9, ErrorMessage = "Invalid Regon number."), MinLength(9)]
        public string Regon { get; set; }
        [Required]
        [MaxLength(256, ErrorMessage = "Name is too long.")]
        public string CompanyName { get; set; }
        [Required]
        [MaxLength(9, ErrorMessage = "Invalid Zip Code"), MinLength(5, ErrorMessage = "Invalid Zip Code")]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Invalid City, name is too long.")]
        public string City { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Invalid Country, name is too long.")]
        public string Country { get; set; }
        [Required]
        [StringLength(70, ErrorMessage = "Invalid Adress, address is too long.")]
        public string Adress { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage ="Invalid password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords dosn't match!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
