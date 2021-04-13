using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MgmtDbContext context;

        public HomeController(ILogger<HomeController> logger, MgmtDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Calendar()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //cut whis reservedTime functions and create new controller for them.
        [HttpGet]
        public IActionResult ReservedTime()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReservedTime(ReservedTime model)
        {
            var modelToDb = new ReservedTime
            {
                Date = model.Date.ToString("dd, MM") //I have to use normal sql query to add date in databese
                                                     //and changing ReservedTime table into table with appropiate column type
            };
            context.ReservedTimes.Add(model);
            context.SaveChanges();
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
