using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
