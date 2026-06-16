using Microsoft.AspNetCore.Mvc;
using RealtimeChat.DTOs.Auth;
using RealtimeChat.Interfaces;

namespace RealtimeChat.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _userService.RegisterAsync(request);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _userService.LoginAsync(request);

            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var response = await _userService.RefreshTokenAsync(refreshToken);

            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}
