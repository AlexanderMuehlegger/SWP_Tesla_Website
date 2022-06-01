using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models {
    public class Email {
        public string to { get; set; }
        public string from { get; set; }
        public string Subject { get; set; }
        public string msg { get; set; }
    }
}
