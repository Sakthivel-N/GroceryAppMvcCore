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
        //5.Feedback
        //6.Logout


        //API URL ADDED
        public static string baseURL;
        private readonly IConfiguration _configuration;
        public List<Product> productsss;

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

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ViewProducts(int id, int msg)
        {

            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                var products = await GetProducts();
                List<Product> ProductList = products.Where(m=>m.Qty > 10).ToList();
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
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
                return View(ProductList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> AddToCart(int ProdId, int CatId)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                int Msg = 0;
                int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                Product product = await GetProducts(ProdId);
                List<Cart> CartData = await GetCarts();
                var cartPresent = CartData.FirstOrDefault(m => m.ProductId == ProdId & m.IsOrdered == false & m.UserId == UserId);
                if (cartPresent != null)
                {
                    Cart cart = cartPresent;
                    cart.PurchasedQty += 1;
                    cart.SubTotalPrice = cart.PurchasedQty * product.Price;
                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PutAsync(baseURL + "/api/Carts/" + cart.CartId, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (apiResponse != null)
                                Msg = 1;
                        }

                    }
                    return RedirectToAction("ViewProducts", new { id = CatId, msg = Msg });

                }
                else
                {
                    Cart cart = new Cart();
                    //cart.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                    cart.UserId = UserId;
                    cart.PurchasedQty = 1;
                    cart.ProductId = product.ProductId;
                    cart.SubTotalPrice = product.Price;
                    cart.IsOrdered = false;



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
                    return RedirectToAction("ViewProducts", new { id = CatId, msg = Msg });
                }




            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


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
        public async Task<Cart> GetCarts(int id)
        {

            Cart received = new Cart();


            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync(baseURL + "/api/Carts/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        received = JsonConvert.DeserializeObject<Cart>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }

            }
            return received;

        }
        [HttpGet]
        public async Task<IActionResult> ViewCart(int msg)
        {

            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Error = null;

                if (msg == 1)
                {
                    ViewBag.Error = "Plz enter correct payment details";
                }
                else if(msg == 2)
                {
                    ViewBag.Error = "Your cart is empty !!";
                }

                List<Cart> carts = await GetCarts();

                ViewBag.ProductList = await GetProducts();

                return View(carts);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                try
                {
                    //var accessEmail = HttpContext.Session.GetString("Email");
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    var httpClient = new HttpClient(clientHandler);
                    var response = await httpClient.DeleteAsync(baseURL + "/api/Carts/" + id);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction(nameof(ViewCart));
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

        public async Task<IActionResult> Quantity(int CartId, int Qty)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            if (HttpContext.Session.GetString("UserName") != null)
            {
                Cart cart = await GetCarts(CartId);
                Product product = await GetProducts(cart.ProductId);


                cart.PurchasedQty = Qty;
                cart.SubTotalPrice = cart.PurchasedQty * product.Price;
                cart.IsOrdered = false;
                if (cart.PurchasedQty == 0)
                    return RedirectToAction("Delete", new { id = cart.CartId });

                using (var httpClient = new HttpClient())
                {
                    StringContent contents = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync(baseURL + "/api/Carts/" + CartId, contents))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();


                        if (apiResponse != null)
                            ViewBag.Message = " Updated Successfully";
                        else
                            ViewBag.Message = " updation Failed";
                    }

                }
                return RedirectToAction("ViewCart");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }




        public async Task<IActionResult> AddToOrder(string Cartlist, int TV, string name, string card, string exp, string cvv)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            if (HttpContext.Session.GetString("UserName") != null)
            {
                if(Cartlist == null || TV.ToString() == null)
                {
                    return RedirectToAction("ViewCart", new { msg = 2});
                }
                if (Cartlist != null & TV.ToString() != null & name != null & card != null & exp != null & cvv != null)
                {
                    //Cart carts = await GetCarts(); 
                    Order orders = new Order();
                    orders.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

                    orders.CartIdList = Cartlist;
                    orders.PaymentMode = "Online";
                    orders.OrderDate = DateTime.Now.ToString("dd/MM/yyyy");
                    orders.TotalValue = TV;
                    ViewBag.Message1 = ViewBag.Message2 = ViewBag.Message3 = 0;


                    var httpClient = new HttpClient();


                    StringContent content = new StringContent(JsonConvert.SerializeObject(orders), Encoding.UTF8, "application/json");

                    //insert order
                    var response = await httpClient.PostAsync(baseURL + "/api/Orders", content);
                    string apiResponse1 = await response.Content.ReadAsStringAsync();
                    Order received = JsonConvert.DeserializeObject<Order>(apiResponse1);



                    string[] cartList = Cartlist.Split(",");
                    cartList = cartList.Take(cartList.Count() - 1).ToArray();
                    foreach (string cart in cartList)
                    {


                        //update cart 
                        Cart cart1 = await GetCarts(Convert.ToInt32(cart));
                        cart1.IsOrdered = true;

                        StringContent content1 = new StringContent(JsonConvert.SerializeObject(cart1), Encoding.UTF8, "application/json");
                        var response1 = await httpClient.PutAsync(baseURL + "/api/Carts/" + cart1.CartId, content1);



                        //update Product
                        Product product = await GetProducts(cart1.ProductId);
                        product.Qty -= cart1.PurchasedQty;
                        StringContent content2 = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                        var response2 = await httpClient.PutAsync(baseURL + "/api/Products/" + cart1.ProductId, content2);



                    }




                    return RedirectToAction("ViewOrders");
                }
                else
                {
                    return RedirectToAction("ViewCart", new { msg = 1 });
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

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
            return RedirectToAction("Index", "Home");
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
        public async Task<List<Delivery>> GetDelivery()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Deliveries");
            List<Delivery> result = JsonConvert.DeserializeObject<List<Delivery>>(JsonStr);
            return result;
        }
        public async Task<List<User>> GetUsers()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(baseURL + "/api/Users");
            List<User> result = JsonConvert.DeserializeObject<List<User>>(JsonStr);
            return result;
        }
        public async Task<IActionResult> ViewOrders()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                OrderView orderView = new OrderView();
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                int UserId = Convert.ToInt32(ViewBag.UserId);
                ViewBag.ProductList = await GetProducts();

                List<Order> orders = await GetOrderView();
                var ordersList = orders.Where(m => m.UserId == UserId).ToList();
                orderView.Orders = ordersList;
                orderView.Carts = await GetCartView();
                orderView.Deliverys = await GetDelivery();
                orderView.Products = await GetProducts();
                orderView.Users = await GetUsers();
                return View(orderView);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                try
                {
                    //var accessEmail = HttpContext.Session.GetString("Email");
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    var httpClient = new HttpClient(clientHandler);
                    var response = await httpClient.DeleteAsync(baseURL + "/api/Orders/" + id);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return RedirectToAction(nameof(ViewOrders));
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
        [HttpGet]
        public ActionResult FeedBack()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> FeedBack(Feedback feedback)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            if (HttpContext.Session.GetString("UserName") != null)
            {
                feedback.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                feedback.FeedbackTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                Feedback cf = new Feedback();

                HttpClientHandler clientHandler = new HttpClientHandler();


                var httpClient = new HttpClient(clientHandler);


                StringContent content = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/Feedbacks", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cf = JsonConvert.DeserializeObject<Feedback>(apiResponse);
                    if (cf != null)
                    {
                        return RedirectToAction("Index");
                    }
                }


                ViewBag.Message = "Feedback not added, Sorry Please try again!!!!!...";
                return View();


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
