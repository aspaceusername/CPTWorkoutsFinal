﻿using Microsoft.AspNetCore.Mvc;

namespace CPTWorkouts.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
