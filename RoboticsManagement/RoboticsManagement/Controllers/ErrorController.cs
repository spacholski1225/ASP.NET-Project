using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Models;
using System.Diagnostics;

namespace RoboticsManagement.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        [AllowAnonymous]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["ErrorMessage"] = "There is nothing";
                    return View("NotFound");
                default:
                    break;

            }
            return NotFound();
        }
    }
}
