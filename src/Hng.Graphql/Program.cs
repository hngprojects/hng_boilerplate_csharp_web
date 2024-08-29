using Hng.Application;
using Hng.Graphql;
using Hng.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterFeatures();

builder.Services
    .AddGraphQLServer()
    .AddGraphQueries()
    .addGraphMutations();

builder.Services.AddApplicationConfig(builder.Configuration);
builder.Services.AddInfrastructureConfig(builder.Configuration.GetConnectionString("DefaultConnectionString"), builder.Configuration.GetConnectionString("RedisConnectionString"));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors();
app.MapGraphQL();

app.Run();

