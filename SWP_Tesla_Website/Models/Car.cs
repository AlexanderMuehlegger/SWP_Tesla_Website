using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SWP_Tesla_Website.Models {
    public class Car {
        public string Model { get; set; }
        public int Ps { get; set; }
        public decimal Acceleration { get; set; }
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
    }
}
