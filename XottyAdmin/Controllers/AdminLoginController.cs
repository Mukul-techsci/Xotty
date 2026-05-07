using Microsoft.AspNetCore.Mvc;

namespace XottyAdmin.Controllers
{
    public class AdminLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
