using Microsoft.AspNetCore.Mvc;
using CPTWorkouts.Models;

namespace CPTWorkouts.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            var model = new AboutViewModel
            {
            };

            return View(model);
        }
    }
}
