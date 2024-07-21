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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<SeederService>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });





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
