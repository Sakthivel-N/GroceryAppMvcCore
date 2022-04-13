using GroceryAppMvcCore.Models;
using Microsoft.AspNetCore.Mvc;

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
        public List<Product> Get

        public IActionResult Index()
        {
            //if(HttpContext.Session.GetString("Email") != null)
            //{
            //    return View();
            //}
            //return RedirectToAction("Index","Home");
            return View();
           
        }
        [HttpGet]
        public IActionResult ViewProducts(int id)
        {
            ViewBag.CategoryId = id;

            return View();
        }



    }
}
