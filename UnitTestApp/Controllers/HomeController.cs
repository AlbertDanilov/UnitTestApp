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

        //sample3
        public IActionResult GetUser(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            User user = repo.Get(id.Value);
            if (user == null)
                return NotFound();
            return View(user);
        }

        public IActionResult AddUser() => View();

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                repo.Create(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
