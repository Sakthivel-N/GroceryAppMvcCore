using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GroceryAppMvcCore.Controllers
{
    public class AdminsController : Controller
    {
        //nav=items
        //1.Index
        //2.ViewCustomers
        //3.ViewProducts
        //4.AddProduct
        //5.ReorderNow
        //6.Employee
        //7.DeliveryHandlers
        //8.Feedbacks
        //9.Logout


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
            return RedirectToAction("Index", "Home");
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

        public async Task<Product> GetProducts(int id)
        {

            Product received = new Product();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Products/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Product>(apiResponse);
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

        public async Task<ActionResult> DeleteProducts(int id)
        {
            try
            {

                HttpClientHandler clientHandler = new HttpClientHandler();
                var httpClient = new HttpClient(clientHandler);
                var response = await httpClient.DeleteAsync(baseURL + "/api/Products/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(ViewProducts));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> DetailProducts(int id)
        {
            Product prod = await GetProducts(id);
            return View(prod);
        }
        public async Task<ActionResult> EditProducts(int id)
        {
            Product product = await GetProducts(id);
            return View(product);
        }


        // POST: TraineesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProducts(int id, Product UpdatedProduct)
        {
            Product pdt = await GetProducts(id);
            UpdatedProduct.ProductId = id;
            UpdatedProduct.Qty = pdt.Qty;



            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(clientHandler);
            StringContent contents = new StringContent(JsonConvert.SerializeObject(UpdatedProduct), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(baseURL + "/api/Products/" + id, contents))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (apiResponse != null)
                    return RedirectToAction("Index");
                else
                    return View();
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

        public async Task<User> GetUsers(int id)
        {


            User receivedusers = new User();

            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(clientHandler);

            using (var response = await httpClient.GetAsync(baseURL + "/api/Users/" + id))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedusers = JsonConvert.DeserializeObject<User>(apiResponse);
                }
                else
                    ViewBag.StatusCode = response.StatusCode;
            }
            return receivedusers;
        }


        public async Task<ActionResult> ViewCustomer()
        {
            List<User> users = await GetUsers();
            return View(users);

        }

        public async Task<ActionResult> DeleteUsers(int id)
        {
            User user = await GetUsers(id);
            return View(user);
        }

        // POST: TraineesController/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUsers(int id, IFormCollection collection)
        {
            try
            {

                HttpClientHandler clientHandler = new HttpClientHandler();
                var httpClient = new HttpClient(clientHandler);
                var response = await httpClient.DeleteAsync(baseURL + "/api/Users/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<List<Feedback>> GetFeedBack()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Feedbacks");
            var result = JsonConvert.DeserializeObject<List<Feedback>>(JsonStr);
            return result;
        }
        public async Task<ActionResult> ViewFeedback()
        {
            List<Feedback> feedback = await GetFeedBack();
            return View(feedback);

        }



        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {

            Product received = new Product();
            //Product ob = new Product();

            //ob.ProductName = "Juice";
            //ob.Qty = 2;
            //ob.Price = 40;
            //ob.CategoryId = 1;
            //ob.ImageUrl = "juice";
            //ob.Description = "mkand";

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/Products", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<Product>(apiResponse);
                    if (received != null)
                    {
                        return RedirectToAction("ViewProducts", "Admins");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReorderNow()
        {
            List<Product> all = await GetProducts();
            var products = all.Where(m => m.Qty <= 10);
            List<Product> Products = new List<Product>();
            foreach (Product product in products)
            {
                Products.Add(product);
            }
            return View(Products);
        }
        [HttpGet]
        public async Task<IActionResult> AddStock(int id)
        {
            Product product = await GetProducts(id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddStock(int id, Product products)
        {
            Product product = await GetProducts(id);
            product.Qty = products.Qty;
            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(clientHandler);
            StringContent contents = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(baseURL + "/api/Products/" + id, contents))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();


                return RedirectToAction("ReorderNow");
            }

        }
    }
}
