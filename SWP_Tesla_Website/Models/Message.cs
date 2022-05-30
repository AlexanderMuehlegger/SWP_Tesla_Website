using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SWP_Tesla_Website.Models {
    public class Message {
        public string msg { get; set; }
        public string status { get; set; }

        public string getJson() {
            string json = JsonConvert.SerializeObject(
                    new {
                        status = this.status,
                        msg = this.msg,
            });
            return json;
        }
    }
}
