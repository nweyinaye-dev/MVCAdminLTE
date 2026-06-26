using Microsoft.AspNetCore.Mvc;
using MSS.Domain.Features.Auth;
using MVCAdminLTE.ApiServices;
using MVCAdminLTE.Models;

namespace MVCAdminLTE.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthApiService _authApiService;

        public LoginController(AuthApiService authApiService)
        {
            _authApiService = authApiService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginRequest = new AuthRequest
                {
                    Username = model.Username,
                    Password = model.Password
                };

                var authResponse = await _authApiService.LoginAsync(loginRequest);
            }

                return RedirectToAction("Index", "Home");
        }
    }
}
