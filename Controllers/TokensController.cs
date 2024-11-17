using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Role))
            {
                return BadRequest("Invalid input");
            }

            var token = _tokenService.GenerateToken(request.UserId, request.Role);
            return Ok(new { Token = token });
        }
        public class TokenRequest
        {
            public string UserId { get; set; }
            public string Role { get; set; }
        }

    }
}
