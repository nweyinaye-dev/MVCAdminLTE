using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MSS.Domain.Features.Auth;
using System.Security.Claims;

namespace MSS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            var response = await _authService.LoginAsync(request);

            if (response is null)
                return Unauthorized(new { Message = "Invalid username or password." });

            return Ok(response);
        }
    }
}
