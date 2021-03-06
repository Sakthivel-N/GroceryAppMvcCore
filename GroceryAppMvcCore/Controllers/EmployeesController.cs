using GroceryAppMvcCore.LogData;
using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GroceryAppMvcCore.Controllers
{
    public class EmployeesController : Controller
    {
        //nav - items
        //1.JobList
        //2.OurJobs
        //3.JobHistory

        public static string baseURL;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _loggerManager;
        public EmployeesController(IConfiguration configuration, ILoggerManager loggerManager)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");
            _loggerManager = loggerManager;
           

        }

        //---------------

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                ViewBag.EmployeeName = HttpContext.Session.GetString("EmployeeName").ToString();
                _loggerManager.LoginInfo("The Employee"+ViewBag.EMployeeName+"Entering Employee dashboard");
                ViewBag.EmployeeId = HttpContext.Session.GetInt32("EmployeeId");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        public async Task<List<User>> GetUsers()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Users");
            var result = JsonConvert.DeserializeObject<List<User>>(JsonStr);
            return result;
        }
        public async Task<List<Delivery>> GetDelivery()
        {
            List<Delivery> received = new List<Delivery>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/Deliveries/"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<List<Delivery>>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return received;
        }
        public async Task<Delivery> GetDelivery(int id)
        {
            Delivery received = new Delivery();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/Deliveries/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Delivery>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }

            return received;
        }

        public async Task<List<Order>> GetOrderView()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Orders");
            List<Order> result = JsonConvert.DeserializeObject<List<Order>>(JsonStr);
            return result;
        }
        public async Task<List<Cart>> GetCarts()
        {

            List<Cart> received = new List<Cart>();


            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/Carts/"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<List<Cart>>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return received;
        }
        public async Task<List<Product>> GetProducts()
        {

            List<Product> received = new List<Product>();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Products/"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }

            return received;

        }

        [HttpGet]
        public async Task<IActionResult> OurJobs(int id)
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                ViewBag.EmployeeName = HttpContext.Session.GetString("EmployeeName").ToString();
                _loggerManager.LoginInfo("The Employee" + ViewBag.EmployeeName + "Entering Pending Jobs");
                ViewBag.Empid = HttpContext.Session.GetInt32("EmployeeId");
                ViewBag.Status = id;
                JobView obj = new JobView();
                obj.deliveries = await GetDelivery();
                obj.orders = await GetOrderView();
                obj.carts = await GetCarts();
                obj.products = await GetProducts();
                obj.users = await GetUsers();   
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        public async Task<IActionResult> Delivered(int id)
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                ViewBag.EmployeeName = HttpContext.Session.GetString("EmployeeName").ToString();
                _loggerManager.LoginInfo("The Employee" + ViewBag.EmployeeName + " Delivered a Job");
                Delivery delivery = await GetDelivery(id);
                delivery.DeliveryDate = DateTime.Now.ToString("dd/MM/yyyy");
                delivery.Status = true;

                using (var httpClient = new HttpClient())
                {
                    StringContent contents = new StringContent(JsonConvert.SerializeObject(delivery), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync(baseURL + "/api/Deliveries/" + id, contents))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                    return RedirectToAction("OurJobs", new { id = 0 });

                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                _loggerManager.LoginInfo("The Employee" + HttpContext.Session.GetString("EmployeeName").ToString() + " Logout successfully");
                HttpContext.Session.Remove("EmployeeName");
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> JobList()
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                ViewBag.EmployeeName = HttpContext.Session.GetString("EmployeeName").ToString();
                _loggerManager.LoginInfo("The Employee" + ViewBag.EmployeeName + " viewing available jobs");
                ViewBag.EmployeeId = HttpContext.Session.GetInt32("EmployeeId");
                JobView obj = new JobView();
                obj.deliveries = await GetDelivery();
                obj.orders = await GetOrderView();
                obj.carts = await GetCarts();
                obj.products = await GetProducts();
                obj.users = await GetUsers();
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public async Task<IActionResult> Accept(int id)
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                Delivery delivery = new Delivery();
                _loggerManager.LoginInfo("The Employee" + ViewBag.EmployeeName + " Accept a Job");
                delivery.OrderId = id;
                delivery.PickupDate = DateTime.Now.ToString("dd/MM/yyyy");
                delivery.Status = false;
                delivery.DeliveryDate = "--";
                delivery.EmployeeId = Convert.ToInt32(HttpContext.Session.GetInt32("EmployeeId")); 

                Delivery received = new Delivery();

                using (var httpClient = new HttpClient())
                {
                    StringContent contents = new StringContent(JsonConvert.SerializeObject(delivery), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(baseURL + "/api/Deliveries", contents))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Delivery>(apiResponse);
                    }

                    return RedirectToAction("OurJobs");
                    //return received;
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}
