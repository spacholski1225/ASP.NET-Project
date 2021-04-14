using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.ViewModels
{
    public class ComplaintFormViewModel
    {
        public string Company { get; set; }
        
        public string Description { get; set; }

        public ERobotsCategory ERobotsCategory { get; set; }
    }
}
