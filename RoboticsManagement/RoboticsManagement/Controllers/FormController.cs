using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Configuration;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.ViewModels;
using System;
using System.Threading.Tasks;

namespace RoboticsManagement.Data
{
    [Authorize(Roles = "Client")]
    public class FormController : Controller
    {
        private readonly MgmtDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FormController> _logger;
        private readonly AutoMapperConfig _mapper;

        public FormController(MgmtDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<FormController> logger,
            AutoMapperConfig mapper)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Form() => View();

        [HttpPost]
        public async Task<IActionResult> Form(FormViewModel model, string name)
        {
            if (ModelState.IsValid && name != null)
            {
                var user = await _userManager.FindByNameAsync(name);
                if (user != null)
                {
                    var summary = _mapper.MapApplicationUserToSummaryViewModel(user);
                    summary.ERobotsCategory = model.ERobotsCategory;
                    summary.Description = model.Description;
                    return RedirectToAction("Summary", "Form", summary);
                }
                else
                {
                    _logger.LogWarning("Usermanager can't find user named " + name);
                }
                _logger.LogWarning("Invalid form modelstate or user name is null");
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Summary(SummaryViewModel summary) => View(summary);

        [HttpPost]
        public async Task<IActionResult> Summary(SummaryViewModel summary, bool isOkay)
        {
            if (ModelState.IsValid && isOkay)
            {
                var model = _mapper.MapSummaryViewModelToFormModel(summary);
                model.CreatedDate = DateTime.Now;
                model.ApplicationUser = await _userManager.FindByIdAsync(summary.UserId);

                var empTask = _mapper.MapSummaryViewModelToEmployeeTask(summary);
                empTask.isDone = false;

                var clientNoti = new ClientNotifications
                {
                    CreatedDate = DateTime.Now,
                    FromRole = ERole.Admin,
                    ToClientId = summary.UserId,
                    IsRead =false,
                    NotiBody = "Hello your form is sent.",
                    NotiHeader = "Form confirmation"
                };
                var adminNoti = new AdminNotifications
                {
                    CreatedDate = DateTime.Now,
                    FromUserId = summary.UserId,
                    ToRole = ERole.Admin,
                    IsRead = false,
                    NotiBody = "Client with id " + summary.UserId + "sent form. ",
                    NotiHeader = "New form from Client " + summary.Company
                };
                try
                {
                    _context.ClientNotifications.Add(clientNoti);
                    _context.AdminNotifications.Add(adminNoti);
                    _context.EmployeeTasks.Add(empTask);
                    _context.complaintFormModels.Add(model);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while adding to database", e);
                    Console.WriteLine(e.Message); // add alert in view
                }
                return RedirectToAction("Success", "Success");
            }
            else
            {
                _logger.LogWarning("Invalid modelstate in summary");
            }
            return View(summary);
        }


    }
}
