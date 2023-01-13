using Microsoft.EntityFrameworkCore;
using openglclevel_server_data.DataAccess;
using openglclevel_server_models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string _authUrl = builder.Configuration.GetValue<string>("security:authURL");

//Google search: services AddScopped instanciate
//Initialize the Instances within ConfigServices in Startup .NET
//https://www.thecodebuzz.com/initialize-instances-within-configservices-in-startup/
builder.Services.AddSingleton<AuthURLValue>(op =>
{
    AuthURLValue obj = new AuthURLValue();
    obj.UrlValue = _authUrl;
    return obj;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<OpenglclevelContext>(options => options.
       UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
