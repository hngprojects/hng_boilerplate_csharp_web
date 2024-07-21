using Hng.Domain.Models;
using Hng.Infrastructure.Services;
using Hng.Web.Bootstrappers;
using Hng.Web.Extensions;
using Hng.Web.Services;
using NLog.Web;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//map appsettings.json to Appsettings class
var appSettings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();

builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDependencies(appSettings)
    .AddConfiguredServices(appSettings.ConnectionStrings.DefaultConnectionString);

builder.Services.AddScoped<SeederService>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

//only seed db if using actual implementation
if (!appSettings.Settings.UseMockForDatabase)
{
    await app.MigrateAndSeed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalErrorHandler(app.Environment);

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
