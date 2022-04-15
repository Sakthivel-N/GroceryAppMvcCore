using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GroceryAppMvcCore.Controllers
{
    public class UsersController : Controller
    {
            //navbar items
                //1.Index
                //2.ViewProducts
                //3.ViewCart
                //4.ViewOrders
                //5.Logout


        //API URL ADDED
        public static string baseURL;
        private readonly IConfiguration _configuration;
        
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

        }

        //---------------
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


                using (var response = await httpClient.GetAsync(baseURL + "/api/Products/"+id))
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

        [HttpGet]
        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("Email") != null)
            //{
            //    return View();
            //}
            //return RedirectToAction("Index", "Home");

            
                ViewBag.Message = HttpContext.Session.GetInt32("UserId");
                ViewBag.Msg = HttpContext.Session.GetString("UserName");


            return View();
            
           
        }

        [HttpGet]
        public async Task<IActionResult> ViewProducts(int id, int msg)
        {
            
            List<Product> products = await GetProducts();
            ViewBag.CategoryId = id;
            ViewBag.Msg = null;
            if (msg == 1)
            {
                ViewBag.Msg = "Product Added to Cart !!";
            }
            else if (msg == 0)
            {
                ViewBag.Msg = "Failed to Add Cart";
            }
            return View(products);
        }
        
        public async Task<IActionResult> AddToCart(int ProdId, int CatId)
        {
            Product product = await GetProducts(ProdId);
            Cart cart = new Cart();
            //cart.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            cart.UserId = 1;
            cart.PurchasedQty = 1;
            cart.ProductId = product.ProductId;
            cart.SubTotalPrice = product.Price;
            cart.IsOrdered = false;

            int Msg = 0;
            Cart received = new Cart();
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/Carts", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<Cart>(apiResponse);
                    if (received != null)
                        Msg = 1;
                }

            }


            return RedirectToAction("ViewProducts",new { id=CatId,msg=Msg});

        }

        
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                if (HttpContext.Session.GetString("UserName") != null)
                {
                    HttpContext.Session.Remove("UserName");
                }
                HttpContext.Session.Remove("UserId");
            }
            return RedirectToAction("Index","Home");
        }
        public async Task<List<Order>> GetOrderView()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Orders");
            List<Order> result = JsonConvert.DeserializeObject<List<Order>>(JsonStr);
            return result;
        }

        public async Task<List<Cart>> GetCartView()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Carts");
            List<Cart> result = JsonConvert.DeserializeObject<List<Cart>>(JsonStr);
            return result;
        }

        public async Task<IActionResult> ViewOrders()
        {
            OrderView orderView = new OrderView();
            orderView.Orders = await GetOrderView();
            orderView.Carts = await GetCartView();
            return View(orderView);

        }

    }
}
