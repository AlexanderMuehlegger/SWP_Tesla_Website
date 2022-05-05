using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWP_Tesla_Website.Models.DB;
using SWP_Tesla_Website.Models;
using Microsoft.AspNetCore.Http;
using System.Data.Common;

namespace SWP_Tesla_Website.Controllers {
    public class CarController : Controller {

        private IRepositoryCar _rep_car = new RepositoryCarDB();
        public IActionResult Index() {
            return View();
        }

        public IActionResult ModelS() {
            return View();
        }

        public async Task<IActionResult> Model3Async() {
            string modell = "Model 3 | Standard Range";
            Car car;
            car = await GetByModelAsync(modell);
            return View(car);

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

        public async Task<List<Car>> GetCarListAsync() {
            HttpContext.Session.Remove("car-error");
            List<Car> cars;


            try {
                await _rep_car.ConnectAsync();

                cars = await _rep_car.GetAllAsync();

                return cars;

            } catch (Exception ex) {
                HttpContext.Session.SetString("car-error", "Car data couldn't be loaded!");
                return null;
            } finally {
                await _rep_car.DisconnectAsync();
            }

        }

        public async Task<Car> GetByModelAsync(string model) {
            Car car;
            try {
                await _rep_car.ConnectAsync();
                car = await _rep_car.GetByModelAsync(model);
                return car;
            } catch (Exception ex){
                HttpContext.Session.SetString("car-error", "Car data couldn't be loaded!");
                return null;
            } finally {
                await _rep_car.DisconnectAsync();
            }
        }


    }
}
