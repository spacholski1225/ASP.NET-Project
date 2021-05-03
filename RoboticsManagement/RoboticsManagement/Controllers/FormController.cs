using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
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

        public FormController(MgmtDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<FormController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
                    var summary = new SummaryViewModel
                    {
                        Adress = user.Adress,
                        City = user.City,
                        Company = user.CompanyName,
                        Country = user.Country,
                        ZipCode = user.ZipCode,
                        ERobotsCategory = model.ERobotsCategory,
                        Description = model.Description,
                        Id = user.Id
                    };
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
                var model = new FormModel
                {
                    Adress = summary.Adress,
                    City = summary.City,
                    Company = summary.Company,
                    Country = summary.Country,
                    ZipCode = summary.ZipCode,
                    ERobotsCategory = summary.ERobotsCategory,
                    Description = summary.Description,
                    CreatedDate = DateTime.Now,
                    ApplicationUser = await _userManager.FindByIdAsync(summary.Id)
                };
                var empTask = new EmployeeTask
                {
                    Adress = summary.Adress,
                    City = summary.City,
                    Company = summary.Company,
                    ERobotsCategory = summary.ERobotsCategory,
                    Country = summary.Country,
                    Description = summary.Description,
                    isDone = false
                };
                try
                {
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
