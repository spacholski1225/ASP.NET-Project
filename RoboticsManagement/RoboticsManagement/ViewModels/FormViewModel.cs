using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
