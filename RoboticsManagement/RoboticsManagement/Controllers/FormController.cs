using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;

namespace RoboticsManagement.Data
{
    public class FormController : Controller
    {
        private readonly MgmtDbContext context;

        public FormController(MgmtDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Form(FormViewModel model)
        {
            if(ModelState.IsValid)
            {
                var toAddModel = new FormModel
                {
                    Company = model.Company,
                    ERobotsCategory = model.ERobotsCategory,
                    Description = model.Description
                };
                context.complaintFormModels.Add(toAddModel);
                context.SaveChanges();
                return RedirectToAction("Success", "Success");
            }
            return View();
        }
    }
}
