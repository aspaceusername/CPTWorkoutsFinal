using Microsoft.AspNetCore.Mvc;
using CPTWorkouts.Models;

namespace CPTWorkouts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/Home
        [HttpGet]
        public IActionResult Get()
        {
            var model = new HomeViewModel
            {
                UserName = "User123", // Example data, ensure actual data is populated correctly
                // Add other necessary properties and initialization
            };

            return Ok(model);
        }
    }
}
