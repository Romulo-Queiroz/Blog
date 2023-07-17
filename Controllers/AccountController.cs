using Microsoft.AspNetCore.Mvc;
using Blog.Services;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{

    [AllowAnonymous]  // Permite que o método seja acessado sem autenticação
     [HttpPost("v1/login")]
   public IActionResult Login([FromServices]TokenServices tokenService)
   {
  
    var token = tokenService.GenerateToken(null);
    return Ok(token);
   }

    [Authorize(Roles = "user")] // Permite que o método seja acessado somente com autenticação
    [HttpPost("v1/user")]
    public IActionResult GetUser() => Ok(User.Identity.Name);

    [Authorize(Roles = "author")]
    [HttpPost("v1/author")]
    public IActionResult GetAuthor() => Ok(User.Identity.Name);
    
    [Authorize(Roles = "admin")]
    [HttpPost("v1/admin")]
    public IActionResult GetAdmin() => Ok(User.Identity.Name);
}