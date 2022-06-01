using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWP_Tesla_Website.Models.Service;

namespace SWP_Tesla_Website.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Email() {//Funktioniert nicht.
            EMailSender broad = new EMailSender();
            broad.SendEmailAsync(new Models.Email() {
                msg = "This is test you cool person!"
            });
            return RedirectToAction("index");
        }
    }
}
