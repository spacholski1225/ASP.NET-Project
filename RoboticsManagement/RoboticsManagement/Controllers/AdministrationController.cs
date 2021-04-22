using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MgmtDbContext _context;

        public AdministrationController(UserManager<ApplicationUser> userManager, MgmtDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeRegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser //TODO extend about employee informations
                {
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Success", "Success");
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult DisplayForm()
        {
            var forms = _context.complaintFormModels.OrderBy(x => x.Id).ToList();

            return View(forms);
        }
        [HttpPost]
        public IActionResult DisplayForm(int id)
        {
            var result = _context.complaintFormModels.FirstOrDefault(x => x.Id == id);
            return RedirectToAction("ConcreteForm", "Form", result);
        }
        [HttpGet]
        public IActionResult ConcreteForm(FormModel result)
        {
            return View(result);
        }
    }
}
