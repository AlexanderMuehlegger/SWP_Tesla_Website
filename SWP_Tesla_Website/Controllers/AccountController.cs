using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWP_Tesla_Website.Models;
using Microsoft.AspNetCore.Http;
using System.Dynamic;
using SWP_Tesla_Website.Models.DB;

namespace SWP_Tesla_Website.Controllers {
    public class AccountController : Controller {

        private IRepositoryUser _rep = new RepositoryUserDB();


        public IActionResult Index() {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null || user_string.Length == 0)
                return RedirectToAction("Login");

            Access access = SWP_Tesla_Website.Models.User.getObject(user_string).access;

            if (access == Access.DEV_ADMIN || access == Access.ADMIN || access == Access.DEV)
                return RedirectToAction("AdminPanel");

            return View();
        }

        public IActionResult AdminPanel() {
            return View();
        }

        [HttpGet]
        public IActionResult Login() {
            string user_string = HttpContext.Session.GetString("user");
            HttpContext.Session.Remove("error-login");

            if(user_string == null || user_string.Length == 0)
                return View();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Login(User userData) {
            if(userData == null) 
                return View();

            ValidateLoginData(userData);

            if (ModelState.IsValid) {
                User validUser;
                try {
                    await this._rep.ConnectAsync();
                    validUser = await this._rep.LoginAsync(userData);

                    if (validUser != null) {
                        HttpContext.Session.SetString("user", validUser.getJson());
                        return RedirectToAction("index", "account");
                    }
                    else {
                        userData.access = Access.UNAUTHORIZED;
                        return View(userData);
                    }
                        

                }catch (Exception ex) {
                    HttpContext.Session.SetString("error-login", ex.Message /*"Something went wrong turing login!"*/);
                    userData.access = Access.UNAUTHORIZED;
                    return View(userData);
                }
                finally {
                    await this._rep.DisconnectAsync();
                }
            }
            

            return View(userData);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null)
                return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register() {

            HttpContext.Session.SetString("mode", "login");
            return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Register(User userData) {
            if(userData == null) {
                return RedirectToAction("Register");                
            }
            ValidateRegisterData(userData);

            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    if (await _rep.RegisterAsync(userData))
                        return RedirectToAction("index");
                    else 
                        HttpContext.Session.SetString("error-register", "Account existiert bereits!");
                    
                }catch (Exception ex) {
                    HttpContext.Session.SetString("error-register", "Beim Registrieren ist ein Fehler aufgetreten!");
                }finally {
                    await _rep.DisconnectAsync();
                }
                return View(userData);
            }

            return View("Login", userData);
        }

        private void ValidateRegisterData(User user) {
            if (user == null)
                return;

            if ((user.Username == null) || (user.Username.Trim().Length < 4))
                ModelState.AddModelError("Username", "Der Username muss mindestens 4 Zeichen lang sein!");

            if (user.Email == null || user.Email.Trim().Length < 1)
                ModelState.AddModelError("Email", "Bitte geben sie eine Email an!");

            if ((user.Password == null) || (user.Password.Length < 6))
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 6 Zeichen lang sein!");
        }

        private void ValidateLoginData(User user) {
            if (user == null)
                return;

            if ((user.Username == null) || (user.Username.Trim().Length < 4))
                ModelState.AddModelError("Username", "Der Username muss mindestens 4 Zeichen lang sein!");

            if ((user.Password == null) || (user.Password.Length < 6))
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 6 Zeichen lang sein!");
        }
    }

}
