using Microsoft.AspNetCore.Mvc;

namespace DeliveryScheduleSolution.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
