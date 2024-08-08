using System.Text.Json.Serialization;
using Hng.Web.Extensions;
using NLog.Web;
using Hng.Application;
using Hng.Infrastructure;
using System.Reflection;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs();
builder.Services.AddApplicationConfig(builder.Configuration);
builder.Services.AddInfrastructureConfig(builder.Configuration.GetConnectionString("DefaultConnectionString"));
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type => type.FullName);
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