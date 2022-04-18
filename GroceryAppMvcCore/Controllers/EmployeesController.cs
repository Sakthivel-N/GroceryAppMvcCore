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

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

        }

        //---------------

        public IActionResult Index()
        {
            return View();
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

        [HttpGet]
        public async Task<IActionResult> OurJobs(int id)
        {
            ViewBag.Empid = 1;
            ViewBag.Status = id;
            JobView obj = new JobView();
            obj.deliveries = await GetDelivery();
            obj.orders = await GetOrderView();
            obj.carts = await GetCarts();
            return View(obj);

        }
        public async Task<IActionResult> Delivered(int id)
        {
            Delivery delivery = await GetDelivery(id);
            delivery.DeliveryDate = DateTime.Now.ToString("dd/mm/yyyy");
            delivery.Status = true;

            using (var httpClient = new HttpClient())
            {
                StringContent contents = new StringContent(JsonConvert.SerializeObject(delivery), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Deliveries/" + id, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
                return RedirectToAction("OurJobs", new {id=0});

            }
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("EmployeeName") != null)
            {
                HttpContext.Session.Remove("EmployeeName");
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> JobList()
        {
            
            JobView obj = new JobView();
            obj.deliveries = await GetDelivery();
            obj.orders = await GetOrderView();
            obj.carts = await GetCarts();
            return View(obj);
        }


        public async Task<IActionResult> Accept(int id)
        {
            Delivery delivery = new Delivery();
            
            
            delivery.OrderId = id;
            delivery.PickupDate = DateTime.Now.ToString("dd/mm/yyyy");
            delivery.Status = false;
            delivery.DeliveryDate = "within 3 days";
            delivery.EmployeeId = 1;


            using (var httpClient = new HttpClient())
            {
                StringContent contents = new StringContent(JsonConvert.SerializeObject(delivery), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Deliveries/" , contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                }

                return RedirectToAction("OurJobs");
            }
        }
    }
}
