using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
