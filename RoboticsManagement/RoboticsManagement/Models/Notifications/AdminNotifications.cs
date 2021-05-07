using System;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models.Notifications
{
    public class AdminNotifications
    {
        [Key]
        public int NotiId { get; set; }
        public ERole ToRole { get; set; }
        public string FromUserId { get; set; }
        public string NotiHeader { get; set; }
        public string NotiBody { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
