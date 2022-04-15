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
        public async Task<IActionResult> ViewCart()
        {

            List<Cart> carts = await GetCarts();

            //ViewBag.TotalPrice = TV;

            return View(carts);
        }

        public async Task<ActionResult> Delete(int id)
        {
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
        /*public string Add()
        {
            return "hi";
        }*/
        public async Task<IActionResult> Add(int cartId,int prodId)
        {
            Cart carts = await GetCarts(cartId);
            Product product = await GetProducts(prodId);
            Cart cart = new Cart();
            cart.CartId = carts.CartId;
            cart.UserId = 1;
            cart.ProductId = product.ProductId;
            cart.PurchasedQty = carts.PurchasedQty += 1;
            cart.SubTotalPrice = product.Price * cart.PurchasedQty;
            cart.IsOrdered = false;

            //TotalPrice += cart.SubTotalPrice;

            //var accessToken = HttpContext.Session.GetString("Email");


            HttpClientHandler clientHandler = new HttpClientHandler();
            Cart received = new Cart();
            var httpClient = new HttpClient(clientHandler);
            StringContent contents = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(baseURL + "/api/Carts/" + cartId, contents))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                received = await GetCarts(cartId);

            }
            return RedirectToAction("ViewCart");
            //return View(received);
            /*CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
                product.Quantity--;      //just do it for decrease quantity for add
                context.SaveChanges();

            }

            HttpContext.Session.SetJson("Cart", cart);
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");*/

        }



        //GET /cart/decrease/id
        public async Task<IActionResult> Decrease(int cartId, int prodId)
        {
            Cart carts = await GetCarts(cartId);
            Product product = await GetProducts(prodId);
            if (carts.PurchasedQty > 1) 
            {
                Cart cart = new Cart();
                cart.CartId = carts.CartId;
                cart.UserId = 1;
                cart.ProductId = product.ProductId;
                cart.PurchasedQty = carts.PurchasedQty -= 1;
                cart.SubTotalPrice = product.Price * cart.PurchasedQty;
                cart.IsOrdered = false;

                //TotalPrice += cart.SubTotalPrice;

                //var accessToken = HttpContext.Session.GetString("Email");


                HttpClientHandler clientHandler = new HttpClientHandler();
                Cart received = new Cart();
                var httpClient = new HttpClient(clientHandler);
                StringContent contents = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Carts/" + cartId, contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = await GetCarts(cartId);

                }
                return RedirectToAction("ViewCart");


            }
            return RedirectToAction("ViewCart");
            /* Product product = await context.Products.FindAsync(id);  //add this
             List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
             CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
             if (cartItem.Quantity > 1 && product.Quantity > 1)
             {
                 --cartItem.Quantity;
                 product.Quantity++;   //just do it for increase quantity for add
                 context.SaveChanges();
             }
             else
             {
                 cart.RemoveAll(x => x.ProductId == id);
             }

             if (cart.Count == 0)
             {
                 HttpContext.Session.Remove("Cart");
             }
             else
             {
                 HttpContext.Session.SetJson("Cart", cart);
             }
             return RedirectToAction("Index");*/
        }
        public async Task<IActionResult> ConfirmCart(int Id, int ProId, int Qty)
        {
            //int TotalPrice = 0;
            Cart carts = await GetCarts(Id);
            Product product = await GetProducts(1);
            Cart cart = new Cart();
            cart.CartId = Id;
            cart.UserId = 1;
            cart.ProductId = ProId;
            cart.PurchasedQty = Qty;
            cart.SubTotalPrice = product.Price * Qty;
            cart.IsOrdered = false;

            //TotalPrice += cart.SubTotalPrice;

            //var accessToken = HttpContext.Session.GetString("Email");


            HttpClientHandler clientHandler = new HttpClientHandler();
            Cart received = new Cart();
            var httpClient = new HttpClient(clientHandler);
            StringContent contents = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(baseURL + "/api/Carts/" + Id, contents))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                received = await GetCarts(Id);

            }
            //return RedirectToAction("ViewCart");
            return View(received);



        }


        public async Task<IActionResult> AddToOrder(string Cartlist, int TV)
        {
            //Cart carts = await GetCarts(); 
            Order orders = new Order();
            //cart.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            orders.UserId = 1;
            orders.CartIdList = Cartlist;
            orders.PaymentMode = "Online";
            orders.DeliveryDate = DateTime.Now.AddDays(3).ToString("dd/mm/yyyy");
            orders.TotalValue = TV;


            Order received = new Order();
            
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(orders), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/Orders", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<Order>(apiResponse);
                    if (received != null)
                        ViewBag.Msg = "success";
                }
                string[] cartList = Cartlist.Split(",");
                foreach (string cart in cartList)
                {
                    Cart cart1 = await GetCarts(Convert.ToInt32(cart));


                    //Cart cart1 = new Cart();
                    cart1.IsOrdered = true;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(cart1), Encoding.UTF8, "application/json");
                    using (var response1 = await httpClient.PostAsync(baseURL + "/api/Carts"+cart1.CartId, content1))
                    {
                        Product product = await GetProducts(cart1.ProductId);
                        product.Qty -= cart1.PurchasedQty;
                        StringContent content2 = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                        using (var res = await httpClient.PostAsync(baseURL + "/api/Products"+product.ProductId, content2))
                        {
                            string apiResponse1 = await res.Content.ReadAsStringAsync();
        
                        }

                        string apiResponse = await response1.Content.ReadAsStringAsync();
                       
                    }
                }

            }


            return RedirectToAction("ViewOrders");

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
