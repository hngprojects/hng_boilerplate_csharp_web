using Hng.Domain.Models;
using Hng.Infrastructure.Context;
using Hng.Web.Data;
using Hng.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddConfiguredServices(connString);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        // Configure identity options later
    })
    .AddEntityFrameworkStores<MyDBContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyDBContext>();
    var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    await Seed.SeedOrganisation(context);
    await Seed.SeedUsers(usermanager);
    await Seed.SeedProducts(context);
    await Seed.SeedOrganisationUser(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
