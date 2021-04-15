using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.ViewModels
{
    public class ReservedTimeViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime Finish { get; set; }
    }
}
