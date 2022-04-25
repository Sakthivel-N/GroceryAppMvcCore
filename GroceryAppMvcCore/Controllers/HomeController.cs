using GroceryAppMvcCore.LogData;
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
        private readonly ILoggerManager _loggerManager;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, ILoggerManager loggerManager)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");
            _logger = logger;
            _loggerManager = loggerManager;
            _loggerManager.LoginInfo("Entering Home dashboard");
        }

        //---------------

        
        public IActionResult Index()
        {
            return View();
        }

        //Admin Login

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(Admin admin)
        {
            if(admin.EmailId != null && admin.Password != null)
            {
                Admin admins = await GetValidAdmin(admin.EmailId, admin.Password);
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

        
       
        public async Task<string> UpdateUser()
        {
           Cart cart = new Cart();
            cart.CartId = 37;
            cart.UserId = 1;
            cart.ProductId = 33;
            cart.PurchasedQty = 10;
            cart.SubTotalPrice = 1000;
            cart.IsOrdered = true;
            using (var httpClient = new HttpClient())
            {
                StringContent contents = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(baseURL + "/api/Carts/37", contents))
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
        public async Task<Admin> GetValidAdmin(string EmailId, string Password)
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
            return (admin.FirstOrDefault(m => m.EmailId == EmailId && m.Password == Password));

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
            if(user.UserName==null  || user.EmailId == null  ||  user.PhoneNumber == null || user.Area == null || user.City == null || user.State == null || user.Country == null || user.Pincode == null || user.Password == null)
            {
                ViewBag.Message = "Some Fields are Empty,Plz Enter Your Valid Details";
                return View();
            }

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
                        ViewBag.Success = "Registration Successsfully";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Plz Enter Your Valid Details";
                        return View();
                    }
                }
            }
            
        }


        //Employee Login

        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeLogin(Employee employee)
        {
            Employee employees = new Employee();
            if (employee.EmaiLId != null && employee.Password != null)
            {
                employees = await GetValidEmployee(employee.EmaiLId, employee.Password);
                if (employees != null)
                {
                    
                    HttpContext.Session.SetString("EmployeeName", employees.EmployeeName);
                    HttpContext.Session.SetInt32("EmployeeId", employees.EmployeeId);
                    return RedirectToAction("Index", "Employees");
                }
                else
                {
                    ViewBag.Message = "Incorrect Employee Credentials";
                    return View();
                }

            }
            return View();


        }

        [HttpGet]
        public async Task<Employee> GetValidEmployee(string EmailId, string Password)
        {

            List<Employee> employee = new List<Employee>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "/api/Employees"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                }
            }
            return (employee.FirstOrDefault(m => m.EmaiLId   == EmailId && m.Password == Password));

        }
    }
}