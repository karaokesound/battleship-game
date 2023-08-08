using Microsoft.AspNetCore.Mvc;
using System;

namespace BattleshipGame.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            throw new InvalidProgramException("Test");
            return View();
        }
    }
}
