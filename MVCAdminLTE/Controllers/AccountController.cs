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
        public async Task<IActionResult> Login(Models.LoginViewModel model)
        {

            try
            {
                var user = await _userRepository.GetByUsernameAsync(model.Username);
                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                if (!_passwordHasher.VerifyPassword(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }

                user = await _userRepository.GetUserWithRolesAndPermissionsAsync(user.Id);
                if (user == null)
                {
                    ModelState.AddModelError("", "Error loading user data.");
                    return View(model);
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                foreach (var permission in user.Permissions)
                {
                    claims.Add(new Claim("Permission", permission.Name));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(24)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);


                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View(model);
            }


        }
    }
}
