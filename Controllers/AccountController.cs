using Microsoft.AspNetCore.Mvc;
using Blog.Services;
using SecureIdentity.Password;
using Blog.ViewModels;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
   [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(
                ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList()
            ));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email, password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - Este E-mail já está cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }
    [HttpPost("v1/login")]
   public IActionResult Login([FromServices]TokenServices tokenService)
   {
  
    var token = tokenService.GenerateToken(null);
    return Ok(token);
   }

}