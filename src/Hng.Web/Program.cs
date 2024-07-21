using Hng.Application.Interfaces;
using Hng.Application.Services;
using Hng.Infrastructure.Repository;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services;
using Hng.Web.Extensions;
using Hng.Web.Mappers;
using Hng.Web.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddConfiguredServices(connString);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<INewsLetterSubscriptionRepository, NewsLetterSubscriptionRepository>();
builder.Services.AddScoped<INewsLetterSubscriptionService, NewsLetterSubscriptionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<SeederService>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

await app.MigrateAndSeed();
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
