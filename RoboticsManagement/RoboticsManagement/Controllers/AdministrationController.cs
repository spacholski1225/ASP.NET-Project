using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Configuration;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MgmtDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly ILogger<AdministrationController> _logger;
        private readonly AutoMapperConfig _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITaskForEmployeeRepository _taskForEmployeeRepository;

        public AdministrationController(UserManager<ApplicationUser> userManager,
            MgmtDbContext context,
            RoleManager<IdentityRole> roleManager,
            IEmployeeTaskRepository employeeTaskRepository,
            ILogger<AdministrationController> logger,
            AutoMapperConfig mapper,
            INotificationRepository notificationRepository,
            ITaskForEmployeeRepository taskForEmployeeRepository)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _employeeTaskRepository = employeeTaskRepository;
            _logger = logger;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _taskForEmployeeRepository = taskForEmployeeRepository;
        }
        [HttpGet]
        public IActionResult DisplayForm() => View(_employeeTaskRepository.SortAscById());

        [HttpPost]
        public IActionResult DisplayForm(int id)
        {
            var result = _employeeTaskRepository.GetTaskById(id);
            if (result != null)
                return RedirectToAction("ConcreteForm", "Administration", result);
            return RedirectToAction("DisplayForm", "Administration");
        }
        [HttpGet]
        public IActionResult ConcreteForm(EmployeeTask result) => View(result);
        [HttpPost]
        public IActionResult ConcreteForm(int id)
        {
            var task = _employeeTaskRepository.GetTaskById(id);
            if (task == null)
                return RedirectToAction("ConcreteForm", "Administration");
            var newTask = new EmployeeTaskViewModel
            {
                TaskId = task.Id
            };
            return RedirectToAction("PickEmployee", newTask);
        }
        [HttpGet]
        public async Task<IActionResult> PickEmployee(EmployeeTaskViewModel newTask)
        {
            var emp = await _userManager.GetUsersInRoleAsync("Employee");
            if (emp != null)
            {
                var employees = new List<EmployeeTaskViewModel>();

                foreach (var employee in emp)
                {
                    var mapEmp = _mapper.MapApplicationUserToEmployeeTaskViewModel(employee);
                    mapEmp.TaskId = newTask.TaskId;
                    employees.Add(mapEmp);
                }
                return View(employees);
            }
            else
            {
                _logger.LogWarning("Can't find employees in Employee role, AdministrationController PickEmployee method");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PickEmployee(string employeeId, int taskId)
        {
            if (employeeId != null && !taskId.Equals(null))
            {
                var employee = await _userManager.FindByIdAsync(employeeId);
                var task = _employeeTaskRepository.GetTaskById(taskId);
                if (task != null && employee != null)
                {
                    var model = _mapper.MapEmployeeTaskToEmployeeTaskViewModel(task);
                    model.EmployeeId = employeeId;
                    model.FirstName = employee.FirstName;
                    model.LastName = employee.LastName;
                    model.TaskId = taskId;
                    model.ZipCode = employee.ZipCode;
                    return RedirectToAction("NewTask", model);
                }
                _logger.LogError("Can not find task with id " + taskId + " or employee with id " + employeeId);
                return View();
            }
            else
            {
                _logger.LogError("While sending task to employee occurs error where employeeId or taskId null");
            }
            return View();

        }
        [HttpGet]
        public IActionResult NewTask(EmployeeTaskViewModel model) => View(model);
        [HttpPost]
        public IActionResult NewTask(string employeeId, int taskId) //add something like that user can't get into there without move across all controllers
        {
            if (employeeId != null)
            {
                var entity = new TaskForEmployee
                {
                    EmployeeId = employeeId,
                    TaskId = taskId
                };
                var notifi = new EmployeeNotifications
                {
                    FromRole = ERole.Admin,
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                    NotiBody = "Hi, you have new task to do! Here's problem description: "
                    + _employeeTaskRepository.GetTaskById(taskId).Description,
                    NotiHeader = "New task from " + _employeeTaskRepository.GetTaskById(taskId).Company,
                    ToEmployeeId = employeeId
                };
                try
                {
                    _notificationRepository.AddNotificationsForEmployee(notifi);
                    _taskForEmployeeRepository.AddNewTaskForEmployee(entity);
                    return RedirectToAction("Success", "Success");
                }
                catch (Exception e)
                {
                    _logger.LogError("Task is already in database", e.ToString());
                    return RedirectToAction("Error", "Error");
                }
            }
            return RedirectToAction("Error", "Error");
        }
    }
}
