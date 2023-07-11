using Blog.Data;
using Blog.Services;
var builder = WebApplication.CreateBuilder(args);

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

app.MapControllers();

app.Run();
