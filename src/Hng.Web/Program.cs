using System.Net;
using Hng.Web.Services;
using Microsoft.AspNetCore.Diagnostics;
using NLog;
using NLog.Extensions.Logging;
using NLog.Fluent;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddConfiguredServices(connString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "text/plain";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(contextFeature.Error, "Unhandled exception");

            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
        }
    });
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
