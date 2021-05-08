using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Configuration;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationRepository _notificationRepository;
        private readonly AutoMapperConfig _mapper;

        public NotificationsController(UserManager<ApplicationUser> userManager, INotificationRepository notificationRepository,
            AutoMapperConfig mapper)
        {
            _userManager = userManager;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("/shared/index/{name?}")]
        public async Task<JsonResult> GetNotifications(string name)//todo change something to display notifications after log in
        {
            if (name == null)
                return Json("Error", "Error");

            var user = await _userManager.FindByNameAsync(name);
            List<NotificationViewModel> returnedList = new List<NotificationViewModel>();
            if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var notis = _notificationRepository.GetNotificationsForEmployee(user.Id);
                notis.ForEach(n =>
                {
                    returnedList.Add(_mapper.MapEmployeeNotificationsToNotificationViewModel(n));
                });
                return Json(returnedList);
            }
            else if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var notis = _notificationRepository.GetNotificationsForAdmin(ERole.Admin);
                notis.ForEach(n =>
                {
                    returnedList.Add(_mapper.MapAdminNotificationsToNotificationViewModel(n));
                });
                return Json(returnedList);
            }
            else if (await _userManager.IsInRoleAsync(user, "Client"))
            {
                var notis = _notificationRepository.GetNotificationsForClient(user.Id);
                notis.ForEach(n =>
                {
                    returnedList.Add(_mapper.MapClientNotificationsToNotificationViewModel(n));
                });
                return Json(returnedList);
            }
            else
            {
                return Json("Error", "Error");
            }
        }
       
    }
}
