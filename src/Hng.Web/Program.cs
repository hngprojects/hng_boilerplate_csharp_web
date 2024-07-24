using System.Text.Json.Serialization;
using Hng.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Hng.Application;
using Hng.Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using Hng.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();
builder.Services.AddApplicationConfig(builder.Configuration);
builder.Services.AddInfrastructureConfig(builder.Configuration.GetConnectionString("DefaultConnectionString"));
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddConfigurationSettings(builder.Configuration);

var app = builder.Build();

await app.MigrateAndSeed();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => c.RouteTemplate = "docs/{documentName}/swagger.json");
    app.UseSwaggerUI(e => e.RoutePrefix = "docs");
}

app.UseGlobalErrorHandler(app.Environment);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
