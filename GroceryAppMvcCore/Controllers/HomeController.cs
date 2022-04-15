using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;


namespace GroceryAppMvcCore.Controllers
{
    //nav=items
    //1.UserLogin
    //2.UserRegistration
    //3.AdminLogin


    public class HomeController : Controller
    {
        //API URL ADDED
        public static string baseURL;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");
            _logger = logger;
        }

        //---------------

        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admin admin)
        {
            if(admin.AdminName != null && admin.Password != null)
            {
                Admin admins = await GetValidAdmin(admin.AdminName, admin.Password);
                if(admins != null)
                {
                    HttpContext.Session.SetString("AdminName", admins.AdminName);
                    return RedirectToAction("Index", "Admins");
                }
                else
                {
                    ViewBag.Message = "Incorrect Admin Credentials";
                    return View();
                }
            }
            return View();
        }

        //Admin Login
        [HttpPost]
        public async Task<string> UpdateUser()
        {
            User user = new User();
            user.UserId = 1;
            user.UserName = "Sakthi";
            user.EmailId = "sakthi@gmail.com";
            user.PhoneNumber = "999999990";
            user.Address = "Chennai";
            user.Wallet = 100;
            user.Password = "1234567890";
            using (var httpClient = new HttpClient())
            {
                StringContent contents = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Users/1", contents))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();


                    if (apiResponse != null)
                        return "Success";
                    else
                        return "Failed";
                }

            }
             
        }       



        [HttpGet]
        public async Task<Admin> GetValidAdmin(string AdminName, string Password)
        {

            List<Admin> admin = new List<Admin>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/Admins"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    admin = JsonConvert.DeserializeObject<List<Admin>>(apiResponse);
                }
            }
            return (admin.FirstOrDefault(m => m.AdminName == AdminName && m.Password == Password));

        }


        //User Login

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(User User)
        {
            if (User.EmailId != null && User.Password != null)
            {
                User user = await GetValidUsers(User.EmailId, User.Password);
                if (user != null)
                {

                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.UserName);

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewBag.Message = "Incorrect Email Or Password";
                    return View();
                }
            }

            return View();
        }



        public async Task<User> GetValidUsers(string EmailId, string Password)
        {
            List<User> Users = new List<User>();



            using (var httpclient = new HttpClient())
            {
                using (var response = await httpclient.GetAsync(baseURL + "/api/users"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }

            return (Users.FirstOrDefault(u => u.EmailId == EmailId && u.Password == Password));

        }

        //

        //User Registration

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            user.Wallet = 0;

            User received = new User();

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(baseURL + "/api/Users", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    received = JsonConvert.DeserializeObject<User>(apiResponse);
                    if (received != null)
                    {

                        return RedirectToAction("Index", "Users");
                    }
                }
            }
            ViewBag.Message = "Registration Failed";
            return View();
        }

    }
}