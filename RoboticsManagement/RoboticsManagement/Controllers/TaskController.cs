using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Configuration;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Repositories;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly MgmtDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AutoMapperConfig _mapper;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly IFormRepository _formRepository;
        private readonly ITaskForEmployeeRepository _taskForEmployeeRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public TaskController(ILogger<TaskController> logger, MgmtDbContext context,
            UserManager<ApplicationUser> userManager, AutoMapperConfig mapper, IEmployeeTaskRepository employeeTaskRepository,
            IFormRepository formRepository, ITaskForEmployeeRepository taskForEmployeeRepository,
            IInvoiceRepository invoiceRepository)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _employeeTaskRepository = employeeTaskRepository;
            _formRepository = formRepository;
            _taskForEmployeeRepository = taskForEmployeeRepository;
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> FinishedTasks()
        {
            //todo add error handler
            var listToReturn = new List<TaskViewModel>();
            var tasks = new List<TaskForEmployee>();

            var doneTasks = _employeeTaskRepository.GetDoneTasks();
            if (doneTasks == null)
            {
                return BadRequest();
            }
            doneTasks.ForEach(t =>
            {
                tasks.Add(_taskForEmployeeRepository.GetTaskById(t.Id));
            });

            foreach (var task in tasks)
            {
                var taskToGetUser = _employeeTaskRepository.GetTaskById(task.TaskId);
                var complaintForm = _formRepository.GetFormByUserId(taskToGetUser.AppUserId);

                var user = await _userManager.FindByIdAsync(taskToGetUser.AppUserId);

                var mapTask = _mapper.MapApplicationUserToTaskViewModel(user);

                mapTask.TaskId = taskToGetUser.Id;
                mapTask.ERobotsCategory = complaintForm.ERobotsCategory;
                mapTask.Description = complaintForm.Description;
                mapTask.CreatedDate = complaintForm.CreatedDate.ToString();
                mapTask.Company = complaintForm.Company;

                listToReturn.Add(mapTask);
            }
            return View(listToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> FinishedTasks(string userId)
        {
            if (userId != null)
            {
                var complaintForm = _formRepository.GetFormByUserId(userId);

                var user = await _userManager.FindByIdAsync(userId);
                var taskViewModel = _mapper.MapApplicationUserToTaskViewModel(user);
                taskViewModel.ERobotsCategory = complaintForm.ERobotsCategory;
                taskViewModel.Description = complaintForm.Description;
                taskViewModel.CreatedDate = complaintForm.CreatedDate.ToString();
                taskViewModel.Company = complaintForm.Company;

                return RedirectToAction("ConcreteTask", "Task", taskViewModel);
            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult ConcreteTask(TaskViewModel taskViewModel)
        {
            return View(taskViewModel);
        }
        [HttpPost]
        public ContentResult ConcreteTask(TaskViewModel taskViewModel, bool isSure)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TaskViewModel));
            var writer = new StringWriter();
            serializer.Serialize(writer, taskViewModel);
            var xmlString = writer.ToString();
            try
            {
                _invoiceRepository.AddInvoice(new InvoiceData { InvoiceXML = xmlString });
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot add invoice date to database!", ex);
            }
            return new ContentResult
            {
                Content = xmlString
            };
        }

    }
}

