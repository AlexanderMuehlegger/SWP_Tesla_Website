using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SWP_Tesla_Website.Models;
using SWP_Tesla_Website.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Controllers {
    public class OrderController : Controller {
        public IRepositoryOrder _repOrder = new RepositoryOrderDB();

        
        public IActionResult Index() {
            if(!hasAccess(Access.USER))
                return RedirectToAction("login", "account");
            return View();
        }

        public async Task<IActionResult> orders() {
            if (!hasAccess(Access.USER))
                return RedirectToAction("login", "account");

            List<Order> orders = await _repOrder.GetAllOrders(getCurrentUser().ID);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> PayOrder(int id) {
            Order order = await _repOrder.GetOrder(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> PayOrder(Order order) {
            bool status = await _repOrder.PayOrder(order);
            return RedirectToAction("orders");
        }

        public async Task<IActionResult> CancelOrder (int id) {
            if (!hasAccess(Access.USER))
                return RedirectToAction("login", "account");
            List<Order> orders = await _repOrder.GetAllOrders(getCurrentUser().ID);

            foreach(Order ord in orders) {
                if(ord.ID == id) {
                    await _repOrder.ChangeStatus(id, OrderStatus.canceled);
                }
            }
            return RedirectToAction("index");
        }

        public User getCurrentUser() {
            string raw = HttpContext.Session.GetString("user");
            return SWP_Tesla_Website.Models.User.getObject(raw);
        }
        public bool hasAccess(Access needed) {
            Access access = getCurrentAccess();

            return access.hasAccess(needed);
        }

        public Access getCurrentAccess() {
            string user_string = HttpContext.Session.GetString("user");
            if (user_string == null || user_string.Length == 0)
                return Access.UNAUTHORIZED;

            return SWP_Tesla_Website.Models.User.getObject(user_string).access;
        }

    }
}
