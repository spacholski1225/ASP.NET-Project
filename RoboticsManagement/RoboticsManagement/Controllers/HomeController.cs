using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
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
        private readonly IConfiguration config;

        public HomeController(ILogger<HomeController> logger, MgmtDbContext context, IConfiguration config)
        {
            _logger = logger;
            this.context = context;
            this.config = config;
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
        public IActionResult ReservedTime(ReservedTimeViewModel model)
        {
            
            /*using (SqlConnection connection = new SqlConnection(config.GetConnectionString("MyConnection")))
            {
                var query = "INSERT INTO dbo.reservation (id, reservation_date, reservation_start_time, reservation_finish_time) " +
                    "VALUES (@id, @reservation_date, @reservation_start_time, @reservation_finish_time)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", model.Id);
                    command.Parameters.AddWithValue("@reservation_date", model.Date.ToString("yyyy-mm-dd"));
                    command.Parameters.AddWithValue("@reservation_start_time", model.Start.ToString("hh:mm:ss"));
                    command.Parameters.AddWithValue("@reservation_finish_time", model.Finish.ToString("hh:mm:ss"));

                    connection.Open();
                    int result = command.ExecuteNonQuery();


                }
            }*/
            if (ModelState.IsValid)
            {
                var reservation = new ReservedTime
                {
                    Id = model.Id,
                    Date = model.Date.ToString("dd-MM-yyyy"),
                    Start = model.Start.ToString("HH:mm"),
                    Finish = model.Finish.ToString("HH:mm")
                };

                context.ReservedTimes.Add(reservation);
                context.SaveChanges();
                return Ok();
            }
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
