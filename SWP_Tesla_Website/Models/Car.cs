﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SWP_Tesla_Website.Models {
    public class Car {
        public string Model { get; set; }
        public int Ps { get; set; }
        public double Acceleration { get; set; }
        public decimal Price { get; set; }
        public int Max_range { get; set; }
        public int Max_speed { get; set; }

        public String getJson() {
            string json = JsonConvert.SerializeObject(new {
                _car = new Car() {
                    Model = this.Model,
                    Ps = this.Ps,
                    Acceleration = this.Acceleration,
                    Price = this.Price,
                    Max_range = this.Max_range,
                    Max_speed = this.Max_speed
                }
            });
            return json;
        }
<<<<<<< Updated upstream
        public static string getNameFromJson(string jsonobject) {
            JObject json = JObject.Parse(jsonobject);
            string name = json["_car"]["Model"].ToString();

            return name;
        }

        public string getModelName() {
            return this.Model.Split("|")[0].Trim();
        }
=======
>>>>>>> Stashed changes

        public static Car getObject(string jsonObject) {
            JObject json = JObject.Parse(jsonObject);

            return new Car() {
                Model = json["_car"]["Model"].ToString(),
                Ps = ((int)json["_car"]["Ps"]),
<<<<<<< Updated upstream
                Acceleration = ((double)json["_car"]["Acceleration"]),
=======
                Acceleration = ((decimal)json["_car"]["Acceleration"]),
>>>>>>> Stashed changes
                Price = ((decimal)json["_car"]["Price"]),
                Max_range = ((int)json["_car"]["Max_range"]),
                Max_speed = ((int)json["_car"]["Max_speed"])
            };
        }
    }
}
