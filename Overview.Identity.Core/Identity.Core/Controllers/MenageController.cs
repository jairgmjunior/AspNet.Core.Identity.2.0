using Microsoft.AspNetCore.Mvc;

namespace Identity.Core.Controllers
{
    public class MenageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}