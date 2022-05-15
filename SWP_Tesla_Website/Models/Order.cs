using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SWP_Tesla_Website.Models {
    public class Order {
        public int ID { get; set; }
        public int user_id { get; set; }
        public int article_id { get; set; }
        public decimal Saldo { get; set; }
        public decimal Pay { get; set; }
        public OrderStatus status { get; set; }

        public string getJson() {
            string json = JsonConvert.SerializeObject(new {
                _order = this
            });
            return json;
        }
        public static Order getObject(string jsonObject) {
            JObject json = JObject.Parse(jsonObject);

            return new Order() {
                ID = int.Parse(json["_order"]["ID"].ToString()),
                user_id = int.Parse(json["_order"]["user_id"].ToString()),
                article_id = int.Parse(json["_order"]["article_id"].ToString()),
                Saldo = Convert.ToDecimal(json["_user"]["access"].ToString())
            };
        }  
    }


    public enum OrderStatus {
        open, paid, canceled
    }

    public static class Extension {
        public static string getStatusName(this OrderStatus status) {
            switch (status) {
                case OrderStatus.open:
                    return "Open";
                case OrderStatus.paid:
                    return "Paid";
                case OrderStatus.canceled:
                    return "Canceled";
                default:
                    return "UNDEFINED";
            }
        }
    }
}
