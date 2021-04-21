using System;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models.ComplaintForm
{
    public class FormModel
    {

        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Company { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Too many, the limit is 500 letters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Choose your robot")]
        public ERobotsCategory ERobotsCategory { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Adress { get; set; }
    }
}
