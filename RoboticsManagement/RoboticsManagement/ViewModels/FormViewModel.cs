using RoboticsManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class FormViewModel
    {
        [Required]
        [StringLength(500, ErrorMessage = "Too many, the limit is 500 letters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Choose your robot")]
        public ERobotsCategory ERobotsCategory { get; set; }
    }
}
