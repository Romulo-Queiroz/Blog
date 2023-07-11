using Microsoft.AspNetCore.Mvc;
using Blog.Services;


namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{

     [HttpPost("v1/login")]
   public IActionResult Login([FromServices]TokenServices tokenService)
   {
  
    var token = tokenService.GenerateToken(null);
    return Ok(token);
   }
}