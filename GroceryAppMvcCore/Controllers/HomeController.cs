using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseURL = _configuration.GetValue<string>("BaseURL");

        }

        //---------------



        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}