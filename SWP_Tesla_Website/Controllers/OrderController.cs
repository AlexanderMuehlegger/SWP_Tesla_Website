using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SWP_Tesla_Website.Models;
using SWP_Tesla_Website.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Reflection.Metadata;
using SWP_Tesla_Website.Models.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SWP_Tesla_Website.Controllers {
    public class OrderController : Controller {
        public IRepositoryOrder _repOrder = new RepositoryOrderDB();
        public IRepositoryCar _repCar = new RepositoryCarDB();

        private readonly ViewRendererService _viewService;
        private readonly IHostingEnvironment _environment;

        public OrderController(IRazorViewEngine viewEngine, ITempDataProvider dataProvider, IHostingEnvironment environment) {
            this._viewService = new(viewEngine, dataProvider);
            this._environment = environment;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJ0S0d+XE9Bc1RFQmJWfFN0R3NddVp5flRDcDwsT3RfQF9jT35Sd0NnUHxWcHNdQg==");
        }
        
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
        public async Task<IActionResult> Invoice (Order order) {

            if (order == null)
                return RedirectToAction("Index", "account");

            return View(order);
        }

        [HttpGet]
        public async Task<ActionResult<string>> DownloadInvoice(int id) {
            Order order = await _repOrder.GetOrder(id);

            try {
                await _repCar.ConnectAsync();
                Car car = (await _repCar.GetByArticleID(order.article_id));
                order.Model = car.Model;
                order.Saldo = car.Price;
            } catch (Exception ex) {
                return RedirectToAction("index", "account");
            } finally {
                await _repCar.DisconnectAsync();
            }

            if (order.Model == null)
                return RedirectToAction("index", "account");

            string viewPath = "~/Views/order/Invoice.cshtml";
            string msg = await _viewService.RenderViewToStringAsync(this.ControllerContext, viewPath, order);

            HtmlToPdfConverter conv = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

            BlinkConverterSettings settings = new BlinkConverterSettings();
            settings.BlinkPath = Path.Combine(_environment.ContentRootPath, "BlinkBinariesWindows");
            conv.ConverterSettings = settings;

            PdfDocument pdf = conv.Convert(msg, "");


            MemoryStream ms = new MemoryStream();
            pdf.Save(ms);
            pdf.Close(true);

            FileStreamResult result = new FileStreamResult(ms, "application/pdf");
            result.FileDownloadName = "Invoice.pdf";

            return File(ms.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output.pdf");
        }

        [HttpGet]
        public async Task<ActionResult<string>> openEveryOrder () {
            if (!hasAccess(Access.DEV))
                return RedirectToAction("index", "account");
            if(await _repOrder.OpenEveryOrder(OrderStatus.open, OrderStatus.pending)) {
                return new Message() {
                    msg = "Opened every Order!",
                    status = "Success"
                }.getJson();
            }
            return new Message() {
                status = "failed",
                msg = "Failed while opening!"
            }.getJson();
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

        [HttpGet]
        public async Task<ActionResult<string>> OrderNow(string model) {
            if (!hasAccess(Access.USER))
                return new Message() {
                    status = "Unauthorized",
                    msg = "Please login!"
                }.getJson();

            try {
                await _repOrder.OpenConnection();
                Order order = await _repOrder.GetRequiredOrderdata(model);
                order.user_id = getCurrentUser().ID;
                
                if (await _repOrder.AddOrder(order) && await _repOrder.AddConformationKey(order, Order.generateConformationKey(16)))
                    return new Message() {
                        status = "Pending",
                        msg = "Please confirm the order!"
                    }.getJson();
            } catch(Exception ex) {
                return new Message() {
                    status = "Failed",
                    msg = "Creating Order has failed!"
                }.getJson();
            } finally {
                await _repOrder.CloseConnection();
            }
            return new Message() {
                status = "Failed",
                msg = "Something went wrong while creating your order!"
            }.getJson();
        }

        [HttpGet]
        public async Task<ActionResult<string>> ConfirmOrder(string key) {
            
            try {
                await _repOrder.OpenConnection();
                if (await _repOrder.CheckConformationKey(key))
                    return new Message() {
                        msg = "Order confirmed!",
                        status = "Success"
                    }.getJson();
            }catch (Exception ex) {
                Console.WriteLine();
            }finally {
                await _repOrder.CloseConnection();
            }
            return new Message() {
                msg = "Something went wrong turing conformation",
                status = "Failed"
            }.getJson();
        }


        public async Task<ActionResult<string>> CancelOrder (int id) {
            if (!hasAccess(Access.USER))
                return new Message() {
                    status = "Unauthorized",
                    msg = "You must be logged in to cancel your order!"
                }.getJson();

            List<Order> orders = await _repOrder.GetAllOrders(getCurrentUser().ID);

            foreach(Order ord in orders) {
                if(ord.ID == id) {
                    await _repOrder.ChangeStatus(id, OrderStatus.canceled);
                    return new Message() {
                        status = "Success",
                        msg = "Order canceled successfully!"
                    }.getJson();
                }
            }
            return new Message() {
                status = "Failed",
                msg = "No order found!"
            }.getJson();
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
