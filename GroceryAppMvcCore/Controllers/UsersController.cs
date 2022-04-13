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

        //public List<Product> Get

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


        [HttpGet]
        public IActionResult ViewCart(int id)
        {
            //if(HttpContext.Session.GetString("UserId") == id && IsOrdered==false)
            //{
            //    return View();
            //}

            return View();
        }


    }
}
