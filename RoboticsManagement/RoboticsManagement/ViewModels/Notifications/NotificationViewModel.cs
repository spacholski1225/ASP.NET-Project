using System;

namespace RoboticsManagement.ViewModels.Notifications
{
    public class NotificationViewModel
    {
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public string NotiHeader { get; set; }
        public string NotiBody { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
