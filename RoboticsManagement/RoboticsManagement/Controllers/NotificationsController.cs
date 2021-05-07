using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
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

        public NotificationsController(UserManager<ApplicationUser> userManager, INotificationRepository notificationRepository)
        {
            _userManager = userManager;
            _notificationRepository = notificationRepository;
        }
        public async Task<IActionResult> Index(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var notis = _notificationRepository.GetNotificationsForEmployee(user.Id);
                return View(notis);
            }
            else if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var notis = _notificationRepository.GetNotificationsForAdmin(ERole.Admin);
                return View(notis);
            }
            else if (await _userManager.IsInRoleAsync(user, "Client"))
            {
                var notis = _notificationRepository.GetNotificationsForClient(user.Id);
                return View(notis);
            }
            else
            {
                return View("Error", "Error");
            }
        }
        [HttpPost]
        public JsonResult GetNotifications(string employeeId = "0506a940-07ea-4162-8e23-235f3f215225")
        {
            var notifications = new List<EmployeeNotifications>();
            notifications = _notificationRepository.GetNotificationsForEmployee(employeeId);
            return Json(notifications);
        }


    }
}
