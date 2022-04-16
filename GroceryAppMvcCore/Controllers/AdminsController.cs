using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GroceryAppMvcCore.Controllers
{
    public class AdminsController : Controller
    {
            //nav=items
                    //1.Index
                    //2.ViewCustomers
                    //3.ViewProducts
                    //4.AddProduct
                    //5.Feedbacks
                    //6.Logout


        //API URL ADDED
        public static string baseURL;
        private readonly IConfiguration _configuration;
        public AdminsController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

        }

        //---------------


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                ViewBag.Message = HttpContext.Session.GetString("AdminName");
            }

            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                HttpContext.Session.Remove("AdminName");
            }
            return RedirectToAction("Index","Home");
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

        public async Task<ActionResult> ViewProducts()
        {
            List<Product> products = await GetProducts();
            return View(products);
        }
    }
}
