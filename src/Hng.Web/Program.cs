using System.Text.Json.Serialization;
using Hng.Web.Extensions;
using NLog.Web;
using Hng.Application;
using Hng.Infrastructure;
using System.Reflection;
using Prometheus;
using Hng.Web.ModelStateError;
using Microsoft.AspNetCore.Mvc;
using Hng.Web.Filters.Swashbuckle;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }).
    ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
            var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ModelError { Field = e.Key, Message = e.Value.Errors.First().ErrorMessage })
                        .ToList();
            return new BadRequestObjectResult(new ModelStateErrorResponse { Errors = errors });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();
builder.Services.AddApplicationConfig(builder.Configuration);
builder.Services.AddInfrastructureConfig(builder.Configuration.GetConnectionString("DefaultConnectionString"), builder.Configuration.GetConnectionString("RedisConnectionString"));
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<SnakeCaseDictionaryFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type => type.FullName);

    c.SchemaFilter<SnakeCaseDictionaryFilter>();
});

// Add Prometheus
builder.Services.AddSingleton<Counter>(sp =>
{
    return Metrics.CreateCounter("request_count", "Number of requests received", new CounterConfiguration
    {
        LabelNames = new[] { "endpoint" }
    });
});

var app = builder.Build();

await app.MigrateAndSeed();

//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger(c => c.RouteTemplate = "docs/{documentName}/swagger.json");
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "docs";
});

app.UseGlobalErrorHandler(app.Environment);
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Add Prometheus middleware
app.UseMetricServer("/metrics");
app.UseHttpMetrics();

app.Use((context, next) =>
{
    var counter = context.RequestServices.GetRequiredService<Counter>();
    counter.WithLabels(context.Request.Path).Inc();
    return next();
});

app.MapControllers();


app.Run();