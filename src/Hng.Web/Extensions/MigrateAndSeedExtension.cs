using Hng.Infrastructure.Context;
using Hng.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.Extensions
{
    public static class MigrateAndSeedExtension
    {
        public static async Task<WebApplication> MigrateAndSeed(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<SeederService>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            await seeder.SeedUsers();
            await seeder.SeedProfile();
            await seeder.SeedProducts();
            await seeder.SeedOrganisation();
            await seeder.SeedCategory();
            return app;
        }
    }
}