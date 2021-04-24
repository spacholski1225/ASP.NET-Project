using Microsoft.AspNetCore.Authorization;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(UserManager<ApplicationUser> userManager, MgmtDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")] //to delete or modify
        public async Task<IActionResult> AddAdmin()
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin"

                };
                var result = await _userManager.CreateAsync(user, "zaq1@WSX");
                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any(x => x.Name == "Admin"))
                    {
                        var role = new IdentityRole
                        {
                            Name = ERole.Admin.ToString()
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, ERole.Admin.ToString());
                    return RedirectToAction("Success", "Success");
                }
            }
                return Ok();
            
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
            return RedirectToAction("ConcreteForm", "Administration", result);
        }
        [HttpGet]
        public IActionResult ConcreteForm(FormModel result)
        {
            return View(result);
        }

        [HttpGet]
        public IActionResult NewTask(int id)
        {
            var result = _context.complaintFormModels.FirstOrDefault(x => x.Id == id);
            var model = new NewTaskViewModel
            {
                Description = result.Description,
                Adress = result.Adress,
                City = result.City,
                Company = result.Company,
                Country = result.Country
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult NewTask(NewTaskViewModel model)
        {
            if (ModelState.IsValid) //todo implement funcionality to choose emplyee for this task
            {
                var task = new EmployeeTask
                {
                    Description = model.Description,
                    Adress = model.Adress,
                    City = model.Adress,
                    Company = model.Company,
                    Country = model.Company
                };
                try
                {
                    _context.EmployeeTasks.Add(task);
                    _context.SaveChanges();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message); //todo move it into logs
                }
            }
            return View(model);
        }
    }
}
