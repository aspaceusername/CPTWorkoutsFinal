using Microsoft.AspNetCore.Mvc;

namespace CPTWorkouts.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
