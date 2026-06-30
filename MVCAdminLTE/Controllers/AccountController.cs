using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MSS.Domain.Features.Auth;
using MVCAdminLTE.ApiServices;
using MVCAdminLTE.Models;
using System.Security.Claims;

namespace MVCAdminLTE.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AccountController(AuthApiService authApiService)
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


                if (authResponse != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, authResponse.Role),
                        new Claim("AccessToken", authResponse.AccessToken)
                    };

                    if (authResponse.Permissions != null)
                    {
                        foreach (var permission in authResponse.Permissions)
                        {
                            new Claim("Permission", permission);
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Invalid username or password";
            }

            return View(model);

           
        }
    }
}
