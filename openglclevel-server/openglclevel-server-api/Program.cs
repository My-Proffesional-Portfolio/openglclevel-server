using Microsoft.EntityFrameworkCore;
using openglclevel_server_api;
using openglclevel_server_data.DataAccess;
using openglclevel_server_models.API.Security;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string _authUrl = builder.Configuration.GetValue<string>("sec:authURL");

//Google search: services AddScopped instanciate
//Initialize the Instances within ConfigServices in Startup .NET
//https://www.thecodebuzz.com/initialize-instances-within-configservices-in-startup/
//https://www.roundthecode.com/dotnet/how-to-read-the-appsettings-json-configuration-file-in-asp-net-core
var securityKeys = builder.Configuration.GetSection("security").Get<SecurityKeys>();

builder.Services.AddSingleton<ISecurityKeys>((serviceProvider) =>
{
    return securityKeys;
});

builder.Services.InjectServices();
builder.Services.AddDbContext<OpenglclevelContext>(options => options.
       UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomAuthentication(securityKeys.JWT_PrivateKey);

builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();