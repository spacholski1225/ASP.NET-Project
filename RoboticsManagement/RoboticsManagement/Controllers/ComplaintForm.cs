using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Data
{
    public class ComplaintForm : Controller
    {
        private readonly MgmtDbContext context;

        public ComplaintForm(MgmtDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Form(ComplaintFormViewModel model)
        {
            if(ModelState.IsValid)
            {
                var toAddModel = new ComplaintFormModel
                {
                    Company = model.Company,
                    ERobotsCategory = model.ERobotsCategory,
                    Description = model.Description
                };
                context.complaintFormModels.Add(toAddModel);
                context.SaveChanges();
                return Ok();
            }
            return RedirectToAction("Index","Home");
        }
    }
}
