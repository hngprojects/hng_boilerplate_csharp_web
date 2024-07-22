using System.Text.Json.Serialization;
using Hng.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Hng.Application;
using Hng.Infrastructure;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();
builder.Services.AddApplicationConfig();
builder.Services.AddInfrastructureConfig(builder.Configuration.GetConnectionString("DefaultConnectionString"));
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

await app.MigrateAndSeed();

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
