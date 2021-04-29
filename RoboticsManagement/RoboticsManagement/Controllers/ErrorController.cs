using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Models;
using System.Diagnostics;

namespace RoboticsManagement.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    _logger.LogError("Can't find page 404");
                    ViewData["ErrorMessage"] = "There is nothing";
                    return View("NotFound");
                default:
                    break;

            }
            return NotFound();
        }
    }
}
