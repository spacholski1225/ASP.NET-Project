using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        Description = model.Description
                    };
                    return RedirectToAction("Summary", "Form", summary);
                }
                else
                {
                    //add to logs
                }

            }

            return View(model); //add into logs

        }

        [HttpGet]
        public IActionResult Summary(SummaryViewModel summary)
        {
            return View(summary);
        } //po podsumowaniu sprawdzic wyslanie bo cos nie dziala oraz dorobic mozliwosc cofniecia w celu zmiany danych

        [HttpPost]
        public IActionResult Summary(SummaryViewModel summary, bool isOkay)
        {
            if (isOkay)
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
                    CreatedDate = DateTime.Now
                };
                try
                {
                    _context.complaintFormModels.Add(model);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // add to logs / add alert
                }

                return RedirectToAction("Success", "Success");
            }
            else
            {
                //add into logs
            }
            return View(summary);
        }


    }
}
