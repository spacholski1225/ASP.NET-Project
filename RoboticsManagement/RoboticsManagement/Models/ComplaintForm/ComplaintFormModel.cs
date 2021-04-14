using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Models.ComplaintForm
{
    public class ComplaintFormModel
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
    }
}
