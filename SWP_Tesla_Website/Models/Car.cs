using System;
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

        public static string getNameFromJson(string jsonobject) {
            JObject json = JObject.Parse(jsonobject);
            string name = json["_car"]["Model"].ToString();

            return name;
        }

        public string getModelName() {
            return this.Model.Split("|")[0].Trim();
        }

        public Model getArticleID() {
            switch (this.Model) {
                case "Model S | Standard Range": return Models.Model.Model_S_SR;
                case "Model S | Plaid": return Models.Model.Model_S_Plaid;
                case "Model X | Standard Range": return Models.Model.Model_X_SR;
                case "Model X | Plaid": return Models.Model.Model_X_Plaid;
                case "Model Y | Long range": return Models.Model.Model_Y_LR;
                case "Model Y | Performance": return Models.Model.Model_Y_PR;
                case "Model 3 | Standard Range": return Models.Model.Model_3_SR;
                case "Model 3 | Long Range": return Models.Model.Model_3_LR;
                case "Model 3 | Performance": return Models.Model.Model_3_PR;
                default: return Models.Model.UNKNOWN;
            }
        }

        public static Car getObject(string jsonObject) {
            JObject json = JObject.Parse(jsonObject);

            return new Car() {
                Model = json["_car"]["Model"].ToString(),
                Ps = ((int)json["_car"]["Ps"]),
                Acceleration = ((double)json["_car"]["Acceleration"]),
                Price = ((decimal)json["_car"]["Price"]),
                Max_range = ((int)json["_car"]["Max_range"]),
                Max_speed = ((int)json["_car"]["Max_speed"])
            };
        }
    }
}
