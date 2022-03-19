﻿namespace FasterMate.Controllers
{
    using FasterMate.Core.Constants;
    using FasterMate.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewData[MessageConstant.SuccessMessage] = "Success! :)";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}