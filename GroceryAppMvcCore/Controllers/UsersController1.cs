using Microsoft.AspNetCore.Mvc;

namespace GroceryAppMvcCore.Controllers
{
    public class UsersController1 : Controller
    {
        //navbar items
            //1.Index
            //2.ViewProducts
            //3.ViewCart
            //4.ViewOrders
            //5.Logout

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Email") != null)
            {
                return View();
            }
            return RedirectToAction("Index","Home");
           
        }



    }
}
