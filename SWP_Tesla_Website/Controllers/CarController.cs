using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Controllers {
    public class CarController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult ModelS() {
            return View();
        }

        public IActionResult Model3() {
            return View();
        }

        public IActionResult ModelX() {
            return View();
        }

        public IActionResult ModelY() {
            return View();
        }

        public IActionResult SolarRoof() {
            return View();
        }

        public IActionResult SolarPannel() {
            return View();
        }


    }
}
