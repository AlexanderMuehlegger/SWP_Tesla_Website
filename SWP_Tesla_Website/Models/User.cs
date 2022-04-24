using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SWP_Tesla_Website.Models {
    public class User {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Access access { get; set; }

        public string getJson() {
            string json = JsonConvert.SerializeObject(new {
                _user = new User() {
                    Username = this.Username,
                    Email = this.Email,
                    Password = "HIDDEN",
                    access = this.access
                }
            }) ;
            return json;
        }

        public static User getObject(string jsonObject) {
            JObject json  = JObject.Parse(jsonObject);

            return new User() {
                Username = json["_user"]["Username"].ToString(),
                Email = json["_user"]["Email"].ToString(),
                access = (Access)int.Parse(json["_user"]["access"].ToString()),
                Password = "HIDDEN"
            };
        }
    }
}
