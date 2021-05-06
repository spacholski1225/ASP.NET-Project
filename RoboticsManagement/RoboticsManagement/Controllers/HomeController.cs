using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoboticsManagement.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("~/Shared/Index.cshtml")]
        public IActionResult Index() => View();
    }
}
