﻿using Microsoft.AspNetCore.Mvc;
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

        private static string[] allowedModels = { "Model S", "Model 3", "Model X", "Model Y" };

        private IRepositoryCar _rep_car = new RepositoryCarDB();
        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> ModelSAsync() {
            List<Car> carsNedded = await GetCars("Model S");
            return View(carsNedded);
        }

        public async Task<IActionResult> Model3Async() {
            List<Car> carsNedded = await GetCars("Model 3");
            return View(carsNedded);
        }

        public async Task<IActionResult> ModelXAsync() {
            List<Car> carsNedded = await GetCars("Model X");
            return View(carsNedded);
        }

        public async Task<IActionResult> ModelYAsync() {
            List<Car> carsNedded = await GetCars("Model Y");
            return View(carsNedded);
        }

        public IActionResult SolarRoof() {
            return View();
        }

        public IActionResult SolarPannel() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CarOrder(string model) {
            //get whole list to send to new view
            if (!allowedModels.Contains(model))
                return RedirectToAction("Index", "Home");
            return View(await GetCars(model));
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

        public async Task<List<Car>> GetCars(string needed) {
            List<Car> carsAll = await GetCarListAsync();
            List<Car> carsNedded = new List<Car>();
            foreach (Car car in carsAll) {
                if (car.Model.Contains(needed)) {
                    carsNedded.Add(car);
                }
            }
            return carsNedded;
        }


    }
}
