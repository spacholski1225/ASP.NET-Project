using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Data;
using RoboticsManagement.ViewModels;
using ReservedTime = RoboticsManagement.Models.ReservedTime;

namespace RoboticsManagement.Controllers
{
    public class ReservationTimeController : Controller
    {
        private readonly MgmtDbContext _context;

        public ReservationTimeController(MgmtDbContext context)
        {
            _context = context;
        }
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

                _context.ReservedTimes.Add(reservation);
                _context.SaveChanges();
                return Ok();
            }

            return View();
        }
    }
}
