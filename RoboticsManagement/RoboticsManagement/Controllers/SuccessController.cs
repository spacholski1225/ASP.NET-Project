using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class SuccessController : Controller
    {
        public IActionResult Success()
        {
            return View();
        }
    }
}
