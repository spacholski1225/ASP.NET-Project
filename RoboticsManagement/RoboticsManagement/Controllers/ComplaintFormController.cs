using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;

namespace RoboticsManagement.Data
{
    public class ComplaintFormController : Controller
    {
        private readonly MgmtDbContext context;

        public ComplaintFormController(MgmtDbContext context)
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
            return RedirectToAction("Error","Error");
        }
    }
}
