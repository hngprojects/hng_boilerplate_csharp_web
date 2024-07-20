using Hng.Application.Services.Interfaces;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Seeder;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.ServiceExtensions
{
    public static class AppExtension
    {
        public static async Task<WebApplication> ExtendApp(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDBContext>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            dbContext.Database.Migrate();
            await Seeder.Seed(dbContext);
            return app;
        }
    }
}
