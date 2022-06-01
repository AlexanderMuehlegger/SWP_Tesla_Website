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
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;

namespace SWP_Tesla_Website.Controllers {
    public class AccountController : Controller {

        private IRepositoryUser _rep = new RepositoryUserDB();
        private IRepositoryCar _rep_car = new RepositoryCarDB();
        private IRepositoryOrder _rep_order = new RepositoryOrderDB();
        private const string key = "dfg756!@@)(*"; //we can use any 10-12 chars

        [HttpGet]
        public IActionResult Index() {
            if (!hasAccess(Access.USER))
                return RedirectToAction("login");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AdminPanel() {
            if(!hasAccess(Access.ADMIN))
                return RedirectToAction("index"); 

            List<Car> cars = await GetCarListAsync();
            List<User> users = await GetUserListAsync();

            List<List<object>> obj = new List<List<object>>();
            obj.Add(cars.Cast<object>().ToList());
            obj.Add(users.Cast<object>().ToList());

            return View(obj);
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

        [HttpGet]
        public async Task<IActionResult> banUser(int id){
            if (!hasAccess(Access.ADMIN))
                return View("index");

            HttpContext.Session.Remove("ban-error");
            try {
                await _rep.ConnectAsync();
                User target = await _rep.GetByIdAsync(id);
                Access currentAccess = getCurrentAccess();
                if (target.access > currentAccess && currentAccess != Access.DEV)
                    HttpContext.Session.SetString("ban-error", "INSUFFICIENT PERMISSIONS!");
                if(!await _rep.setAccessLevel(id, Access.BANNED))
                    HttpContext.Session.SetString("ban-error", "Something went wrong");

                return RedirectToAction("adminpanel");
            }catch(Exception ex) {
                HttpContext.Session.SetString("ban-error", "Database error occured!");
                return RedirectToAction("adminpanel");
            }finally {
                await _rep.DisconnectAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> unbanUser(int id) {
            if (!hasAccess(Access.ADMIN))
                return RedirectToAction("index");
            HttpContext.Session.Remove("ban-error");

            try {
                await _rep.ConnectAsync();
                User target = await _rep.GetByIdAsync(id);
                if (target.access != Access.BANNED) {
                    HttpContext.Session.SetString("ban-error", "User Not Banned!");
                    return RedirectToAction("index");
                }
                if (await _rep.setAccessLevel(id, Access.USER))
                    return RedirectToAction("index");

            }catch (Exception ex) {
                HttpContext.Session.SetString("ban-error", "An Error occured!");
                
            }finally {
                await _rep.DisconnectAsync();
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> orders() {
            if (!hasAccess(Access.USER))
                return RedirectToAction("login");
            string user_string = HttpContext.Session.GetString("user");
            int user_id = SWP_Tesla_Website.Models.User.getObject(user_string).ID;
            List<Order> orders = await _rep_order.GetAllOrders(user_id);

            return View(orders);
        }


        [HttpPost]
        public async Task<IActionResult> Login(User userData) {
            HttpContext.Session.SetString("LoginState", "login");
            HttpContext.Session.Remove("error-login");

            if (userData == null) 
                return View();
            

            ValidateLoginData(userData);

            if (userData.Username.Contains("@")) {
                userData.Email = userData.Username;
                userData.Username = null;
            }

            if (ModelState.IsValid) {
                User validUser;
                try {
                    await this._rep.ConnectAsync();
                    validUser = await this._rep.LoginAsync(userData);

                    if (validUser != null) {
                        if (validUser.access == Access.BANNED) {
                            HttpContext.Session.SetString("error-login", "You are BANNED!");
                            return RedirectToAction("View");
                        }
                        
                        HttpContext.Session.SetString("user", validUser.getJson());
                        
                        return RedirectToAction("index");
                    } else {
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

        public ActionResult changeAdminPanelState () {
            string currentState = HttpContext.Session.GetString("adminpanel-state");

            HttpContext.Session.SetString("adminpanel-state", currentState == "car" ? "user" : "car");

            return Content(currentState == "car" ? "user" : "car");
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

        public async Task<List<Car>> GetCarListAsync() {
            HttpContext.Session.Remove("car-error");
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

        public async Task<List<User>> GetUserListAsync () {
            HttpContext.Session.Remove("user-error");

            List<User> user;

            try {
                await _rep.ConnectAsync();

                user = await _rep.GetAllUser();

                return user;
            }catch (Exception ex) {
                HttpContext.Session.SetString("user-error", "Users couldn't be loaded!");
                return null;
            }finally {
                await _rep.DisconnectAsync();
            }
        }
        public User getCurrentUser () {
            string raw = HttpContext.Session.GetString("user");
            return SWP_Tesla_Website.Models.User.getObject(raw);
        }
        public bool hasAccess ( Access needed ) {
            Access access = getCurrentAccess();

            return access.hasAccess(needed);
        }

        public Access getCurrentAccess () {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null || user_string.Length == 0)
                return Access.UNAUTHORIZED;

            return SWP_Tesla_Website.Models.User.getObject(user_string).access;
        }
    }

}
