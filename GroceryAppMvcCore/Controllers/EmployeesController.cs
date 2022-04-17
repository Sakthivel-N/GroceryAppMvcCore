using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GroceryAppMvcCore.Controllers
{
    public class EmployeesController : Controller
    {
        //nav - items
        //1.JobList
        //2.OurJobs

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
        public async Task<IActionResult> OurJobs()
        {
            ViewBag.Empid = 1;
            JobView obj = new JobView();
            obj.deliveries= await GetDelivery();
            obj.orders = await GetOrderView();
            obj.carts = await GetCarts();
            return View(obj);

        }
    }
}
