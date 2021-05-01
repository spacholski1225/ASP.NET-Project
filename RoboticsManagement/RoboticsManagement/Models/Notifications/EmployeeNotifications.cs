using System;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models.Notifications
{
    public class EmployeeNotifications
    {
        [Key]
        public int NotiId { get; set; }
        public ERole FromRole { get; set; }
        public string ToEmployeeId { get; set; }
        public string NotiHeader { get; set; }
        public string NotiBody { get; set; }
        public string IsRead { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
