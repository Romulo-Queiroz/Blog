using Microsoft.AspNetCore.Mvc;
using Blog.Services;
using SecureIdentity.Password;
using Blog.ViewModels;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    private readonly IConfiguration _configuration;

    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] EmailService emailService,
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
            emailService.Send(user.Name, user.Email, "Bem-vindo ao Blog", $"<p>Olá {user.Name}, seja bem-vindo ao Blog!</p><p>Seu usuário é: {user.Email}</p><p>Sua senha é: {password}</p>");
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
     [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] BlogDataContext context,
        [FromServices] TokenServices tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(
                ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList()
            ));

        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }
    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
       [FromBody] UploadImageViewModel model,
       [FromServices] BlogDataContext context)
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            string basePath = _configuration["AppSettings:BasePath"];

            // Define caminho completo para pasta de uploads
            string imagesFolder = "UserRoot/images";
            string fullPath = Path.Combine(basePath, imagesFolder,fileName);

            // Salva arquivo em disco
            await System.IO.File.WriteAllBytesAsync(fullPath, bytes);

            // Retorna resposta
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }

        var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

        if (user == null)
            return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

        user.Image = $"https://localhost:0000/images/{fileName}";
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }

        return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!", null));
    }
}