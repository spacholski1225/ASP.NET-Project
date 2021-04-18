using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Range(10,10,ErrorMessage ="Invalid NIP number.")]
        public int NIP { get; set; }
        [Required]
        [Range(9, 9, ErrorMessage = "Invalid Regon number.")]
        public int Regon { get; set; }
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
    }
}
