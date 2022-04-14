using Microsoft.AspNetCore.Mvc;

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
    }
}
