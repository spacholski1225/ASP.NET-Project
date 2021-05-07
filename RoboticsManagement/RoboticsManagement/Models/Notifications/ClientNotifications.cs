using System;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models.Notifications
{
    public class ClientNotifications
    {
        [Key]
        public int NotiId { get; set; }
        public ERole FromRole { get; set; }
        public string ToClientId { get; set; }
        public string NotiHeader { get; set; }
        public string NotiBody { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
