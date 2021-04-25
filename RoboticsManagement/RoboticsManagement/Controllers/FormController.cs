using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public FormController(MgmtDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Form()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Form(FormViewModel model, string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if(ModelState.IsValid)
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
            return View(model);
        }

        [HttpGet]
        public IActionResult Summary(SummaryViewModel summary)
        {
            return View(summary);
        } //po podsumowaniu sprawdzic wyslanie bo cos nie dziala oraz dorobic mozliwosc cofniecia w celu zmiany danych

        [HttpPost]
        public IActionResult Summary(SummaryViewModel summary,bool isOkay = true)
        {
            if(isOkay)
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
                catch(Exception e)
                {
                    Console.WriteLine(e.Message); // todo change this line into log to file or sth else
                }

                return RedirectToAction("Success", "Success");
            }
            return View(summary);
        }

        
    }
}
