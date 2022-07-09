using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnitTestApp.Models;

namespace UnitTestApp.Controllers
{
    public class HomeController : Controller
    {
        //sample1
        //public IActionResult Index()
        //{
        //    ViewData["Message"] = "Hello world!";
        //    return View("Index");
        //}


        //sample2
        IRepository repo;
        public HomeController(IRepository r)
        {
            repo = r;
        }
        public IActionResult Index()
        {
            return View(repo.GetAll());
        }
    }
}
