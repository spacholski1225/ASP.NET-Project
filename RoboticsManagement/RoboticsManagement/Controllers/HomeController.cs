using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("~/Shared/Index.cshtml")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
