using System.Text;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Configuration.JwKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };   
});

builder.Services
.AddControllers()
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenServices>(); //sempre cria uma nova instância um novo Token

//   Além do AddTransient, temos também:
//builder.Services.AddScoped(); // cria um novo Token por transação
//builder.Services.AddSingleton(); // cria uma única instância para toda a aplicação

 
var app = builder.Build();

var smtp = new Configuration.SmtpConfiguration(); //instanciando a classe Smtp configuration 
app.Configuration.GetSection("Smtp").Bind(smtp);   
Configuration.Smtp = smtp;

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
