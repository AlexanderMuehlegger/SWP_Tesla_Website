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
        private IRepositoryCar _rep_car = new RepositoryCarDB();


        public IActionResult Index() {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null || user_string.Length == 0)
                return RedirectToAction("Login");

            Access access = SWP_Tesla_Website.Models.User.getObject(user_string).access;

            if (access.hasAccess(Access.ADMIN))
                return RedirectToAction("AdminPanel");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AdminPanel() {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null || user_string.Length == 0)
                return RedirectToAction("Login");

            Access access = SWP_Tesla_Website.Models.User.getObject(user_string).access;

            if(!access.hasAccess(Access.ADMIN))
                return RedirectToAction("index");

            List<Car> cars = await GetCars();


            return View(cars);
        }

        [HttpGet]
        public IActionResult Login() {

            if (HttpContext.Session.GetString("LoginState") == null)
                HttpContext.Session.SetString("LoginState", "login");

            string user_string = HttpContext.Session.GetString("user");

            if(user_string == null || user_string.Length == 0)
                return View();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Login(User userData) {
            HttpContext.Session.SetString("LoginState", "login");
            HttpContext.Session.Remove("error-login");

            if (userData == null) 
                return View();

            ValidateLoginData(userData);

            if (ModelState.IsValid) {
                User validUser;
                try {
                    await this._rep.ConnectAsync();
                    validUser = await this._rep.LoginAsync(userData);

                    if (validUser != null) {
                        HttpContext.Session.SetString("user", validUser.getJson());
                        return RedirectToAction("index");
                    }
                    else {
                        userData.access = Access.UNAUTHORIZED;
                        return View(userData);
                    }
                        

                }catch (Exception ex) {
                    HttpContext.Session.SetString("error-login", "Something went wrong turing login!");
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
        public IActionResult Logout() {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null)
                return RedirectToAction("Login");
            HttpContext.Session.Remove("user");

            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult Register() {
            HttpContext.Session.SetString("mode", "register");
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Register(User userData) {
            HttpContext.Session.SetString("LoginState", "register");
            HttpContext.Session.Remove("error-register");

            if (userData == null) {
                return RedirectToAction("Login");                
            }
            if (!ValidateRegisterData(userData)) {
                HttpContext.Session.SetString("register-error", "Something went wrong!");
                return View("Login");
            }

            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    if (await _rep.RegisterAsync(userData)) {
                        HttpContext.Session.SetString("user", userData.getJson());
                        return RedirectToAction("index");
                    }else 
                        HttpContext.Session.SetString("error-register", "Account existiert bereits!");
                    
                }catch (Exception ex) {
                    HttpContext.Session.SetString("error-register", "Beim Registrieren ist ein Fehler aufgetreten!");
                }finally {
                    await _rep.DisconnectAsync();
                }
                return View("Login", userData);
            }

            return View("Login", userData);
        }

        private bool ValidateRegisterData(User user) {
            if (user == null)
                return false;

            if ((user.Username == null) || (user.Username.Trim().Length < 4))
                ModelState.AddModelError("Username", "Der Username muss mindestens 4 Zeichen lang sein!");

            if (user.Email == null || user.Email.Trim().Length < 1)
                ModelState.AddModelError("Email", "Bitte geben sie eine Email an!");

            if ((user.Password == null) || (user.Password.Length < 6))
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 6 Zeichen lang sein!");

            return true;
        }

        private bool ValidateLoginData(User user) {
            if (user == null)
                return false;

            if ((user.Username == null) || (user.Username.Trim().Length < 4))
                ModelState.AddModelError("Username", "Der Username muss mindestens 4 Zeichen lang sein!");

            if ((user.Password == null) || (user.Password.Length < 6))
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 6 Zeichen lang sein!");

            return true;
        }

        public async Task<List<Car>> GetCars() {
            List<Car> cars;

            try {
                await _rep_car.ConnectAsync();
                cars = await _rep_car.GetAllAsync();

                return cars;

            }catch (Exception ex) {
                HttpContext.Session.SetString("car-error", "Car data couldn't be loaded!");
                return null;
            }finally {
                await _rep_car.DisconnectAsync();
            }

        }
    }

}
