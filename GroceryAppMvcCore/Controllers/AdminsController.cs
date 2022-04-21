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
        public async Task<IActionResult> DeliveryHandler1()
        {
            DeliveryVuew deliveryView = new DeliveryVuew();
            ViewBag.val = 100;

            deliveryView.Orders = await GetOrder();
            deliveryView.Deliveries = await GetDelivery();


            return View(deliveryView);

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

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                ViewBag.Message = HttpContext.Session.GetString("AdminName");
            }
            else
            {
                return RedirectToAction("Index", "Home");
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


        public async Task<Product> GetProductss(int id)
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
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                List<Product> products = await GetProducts();
                return View(products);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> DeleteProducts(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<ActionResult> DetailProducts(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {

                Product prod = await GetProductss(id);
                return View(prod);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<ActionResult> EditProducts(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Product product = await GetProductss(id);
                return View(product);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }




        public async Task<IActionResult> EditProducts(Product UpdatedProduct)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Product product = await GetProductss(UpdatedProduct.ProductId);
                UpdatedProduct.Qty = product.Qty;
                HttpClientHandler clientHandler = new HttpClientHandler();

                var httpClient = new HttpClient(clientHandler);
                StringContent contents = new StringContent(JsonConvert.SerializeObject(UpdatedProduct), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Products/" + UpdatedProduct.ProductId, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse != null)
                        return RedirectToAction("ViewProducts");
                    else
                        return View();
                }
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
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                List<User> users = await GetUsers();
                return View(users);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }





        public async Task<ActionResult> DeleteUsers(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                try
                {

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    var httpClient = new HttpClient(clientHandler);
                    var response = await httpClient.DeleteAsync(baseURL + "/api/Users/" + id);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("ViewCustomer");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public async Task<IActionResult> AddProduct(Product product)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Product received = new Product();


                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(baseURL + "/api/Products", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Product>(apiResponse);

                        return RedirectToAction("ViewProducts", "Admins");

                    }
                }


            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }


        public async Task<IActionResult> ReorderNow()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddStock(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Product product = await GetProductss(id);
                return View(product);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> AddStock(Product products)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Product product = await GetProductss(products.ProductId);
                product.Qty = products.Qty;
                HttpClientHandler clientHandler = new HttpClientHandler();

                var httpClient = new HttpClient(clientHandler);
                StringContent contents = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Products/" + products.ProductId, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();


                    return RedirectToAction("ReorderNow");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<List<Employee>> GetEmployees()
        {

            List<Employee> received = new List<Employee>();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Employees/"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }
        public async Task<Employee> GetEmployees(int id)
        {

            Employee received = new Employee();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Employees/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Employee>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }


        public async Task<IActionResult> ViewEmployees()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                List<Employee> employees = await GetEmployees();
                return View(employees);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        public async Task<ActionResult> DetailEmployees(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Employee employee = await GetEmployees(id);
                return View(employee);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult AddEmployee()
        {
            if (HttpContext.Session.GetString("AdminName") != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Employee received = new Employee();

                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(baseURL + "/api/Employees", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Employee>(apiResponse);
                        if (received != null)
                        {

                            return RedirectToAction("ViewEmployees");
                        }
                    }
                }
                ViewBag.Message = " Failed To Add Employee ";
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }



        }

        public async Task<ActionResult> EditEmployees(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                Employee employee = await GetEmployees(id);
                return View(employee);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEmployees(Employee UpdatedEmployee)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                //UpdatedEmployee.EmployeeId = UpdatedEmployee;
                //var accessToken = HttpContext.Session.GetString("Email");


                HttpClientHandler clientHandler = new HttpClientHandler();

                var httpClient = new HttpClient(clientHandler);
                StringContent contents = new StringContent(JsonConvert.SerializeObject(UpdatedEmployee), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Employees/" + UpdatedEmployee.EmployeeId, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (apiResponse != null)
                        return RedirectToAction("ViewEmployees");
                    else
                        return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> DeleteEmployees(int id)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                try
                {

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    var httpClient = new HttpClient(clientHandler);
                    var response = await httpClient.DeleteAsync(baseURL + "/api/Employees/" + id);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction(nameof(ViewEmployees));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<List<Order>> GetOrder()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Orders");
            List<Order> result = JsonConvert.DeserializeObject<List<Order>>(JsonStr);
            return result;
        }

        public async Task<List<Delivery>> GetDelivery()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Deliveries");
            List<Delivery> result = JsonConvert.DeserializeObject<List<Delivery>>(JsonStr);
            return result;
        }


        public async Task<IActionResult> DeliveryHandlers(int val)
        {
            if (HttpContext.Session.GetString("AdminName") != null)
            {
                DeliveryVuew deliveryView = new DeliveryVuew();
                ViewBag.val = val;

                deliveryView.Orders = await GetOrder();
                deliveryView.Deliveries = await GetDelivery();
                deliveryView.Users = await GetUsers();
                deliveryView.Products = await GetProducts();
                deliveryView.Carts = await GetCarts();

                return View(deliveryView);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

    }
}
