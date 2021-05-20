using System.Collections.Generic;

namespace RoboticsManagement.ViewModels.Notifications
{
    public class NotificationListViewModel
    {
        public NotificationListViewModel()
        {
            NotificationList = new List<NotificationViewModel>();
        }
        public List<NotificationViewModel> NotificationList { get; set; }
    }
}
