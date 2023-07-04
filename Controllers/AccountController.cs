using Microsoft.AspNetCore.Mvc;
using Blog.Services;


namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
     [HttpPost("v1/login")]
   public IActionResult Login([FromServices]TokenServices tokenService)
   {
    var TokenService = new TokenServices();
    var token = TokenService.GenerateToken(null);

    return Ok(token);
   }
}