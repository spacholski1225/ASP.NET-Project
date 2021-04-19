using RoboticsManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class FormViewModel
    {
        [Required]
        public string Company { get; set; } // there will be account details
        [Required]
        public string Description { get; set; }
        [Required]
        public ERobotsCategory ERobotsCategory { get; set; }
        
    }
}
