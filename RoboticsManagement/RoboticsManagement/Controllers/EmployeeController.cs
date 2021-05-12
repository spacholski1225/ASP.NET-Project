using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly MgmtDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskForEmployeeRepository _taskForEmployeeRepository;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly INotificationRepository _notificationRepository;

        public EmployeeController(MgmtDbContext context,
            UserManager<ApplicationUser> userManager,
            ITaskForEmployeeRepository taskForEmployeeRepository,
            IEmployeeTaskRepository employeeTaskRepository,
            ILogger<EmployeeController> logger,
            INotificationRepository notificationRepository)
        {
            _context = context;
            _userManager = userManager;
            _taskForEmployeeRepository = taskForEmployeeRepository;
            _employeeTaskRepository = employeeTaskRepository;
            _logger = logger;
            _notificationRepository = notificationRepository;
        }
        [HttpGet]
        public IActionResult EmployeeTask(int id)
        {
            var task = _employeeTaskRepository.GetTaskById(id);
            if (task == null)
            {
                _logger.LogWarning("Can't find task in EmployeeTask with id " + id);
                return RedirectToAction("Error", "Error");
            }
            return View(task);
        }
        [HttpGet]
        public async Task<IActionResult> TasksForEmployee(string name)
        {
            var employee = await _userManager.FindByNameAsync(name);
            if (employee != null)
            {
                var tasks = _taskForEmployeeRepository.GetTasksForEmployee(employee.Id);
                if (tasks == null)
                    return View();
                return View(tasks);
            }
            _logger.LogWarning("Employee named " + name + "doesn't exist");
            return View(name);
        }
        public IActionResult DoneTask(int id)
        {
            var task = _employeeTaskRepository.GetTaskById(id);
            if (task != null)
            {
                task.isDone = true;
                var noti = new AdminNotifications
                {
                    CreatedDate = DateTime.Now,
                    FromUserId = _taskForEmployeeRepository.GetTaskById(id).EmployeeId,
                    IsRead = false,
                    NotiBody = "Employee with id " + _taskForEmployeeRepository.GetTaskById(id).EmployeeId
                    + "finished task with id " + id,
                    NotiHeader = "Finished Task #" + id,
                    ToRole = ERole.Admin
                };
                _notificationRepository.AddNotificationsForAdmin(noti);
            }
            else
            {
                _logger.LogWarning("Task with id " + id + "doesn't exist");
            }
            return RedirectToAction("Success", "Success");

        }
    }
}
