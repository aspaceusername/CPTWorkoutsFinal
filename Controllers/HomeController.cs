using Microsoft.AspNetCore.Mvc;
using CPTWorkouts.Models;

namespace CPTWorkouts.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                UserName = "User123", // Example data, ensure actual data is populated correctly
                // Add other necessary properties and initialization
            };

            return View(model);
        }
    }
}
