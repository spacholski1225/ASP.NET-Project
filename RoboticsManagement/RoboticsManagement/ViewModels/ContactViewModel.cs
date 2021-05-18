using RoboticsManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class ContactViewModel
    {
        public string UserName { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Topic { get; set; }
        public EContact EContact  { get; set; }
    }
}
