using System.Net;
using System.Text.Json.Serialization;
using AutoMapper;
using Hng.Application.Interfaces;
using Hng.Application.Services;
using Hng.Infrastructure.Repository;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services;
using Hng.Web.Mappers;
using Hng.Web.Services;
using Hng.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Extensions.Logging;
using NLog.Fluent;
using NLog.Web;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<TokenService>(); // Use AddScoped for services that are instantiated per request
builder.Services.AddScoped<IEmailService>(sp => new EmailService(
    builder.Configuration["Smtp:Server"],
    builder.Configuration["Smtp:User"],
    builder.Configuration["Smtp:Pass"]));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Use config value
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddConfiguredServices(connString);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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
