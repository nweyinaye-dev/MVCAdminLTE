using Microsoft.AspNetCore.Mvc;

namespace MVCAdminLTE.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {

            return RedirectToAction("Index", "Home");
        }
    }
}
