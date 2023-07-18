using Microsoft.AspNetCore.Mvc;
using Blog.Services;
//
using Blog.ViewModels;
using Blog.Data;
using Blog.Models;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
  [HttpPost("v1/accounts")]
  public async Task<ActionResult> Post(
    [FromBody] RegisterViewModel model,
    [FromServices] BlogDataContext context)
  {
    if (!ModelState.IsValid)
      return BadRequest(new ResultViewModel<string>(
        ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage).ToList()
      ));

      var user = new User
      {
        Name = model.Name,
        Email = model.Email,
        Slug = model.Email.Replace("@", "-").Replace(".", "-"),
      };
      await context.Users.AddAsync(user);
      return Ok(new ResultViewModel<string>("Usuário criado com sucesso"));

  }
    [HttpPost("v1/login")]
   public IActionResult Login([FromServices]TokenServices tokenService)
   {
  
    var token = tokenService.GenerateToken(null);
    return Ok(token);
   }

}