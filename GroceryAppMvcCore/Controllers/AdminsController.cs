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
        public IActionResult Index()
        {
            return View();
        }
    }
}
