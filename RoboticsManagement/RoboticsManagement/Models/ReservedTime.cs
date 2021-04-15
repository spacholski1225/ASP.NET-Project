using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Models
{
    public class ReservedTime
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string Finish { get; set; }
    }
}
