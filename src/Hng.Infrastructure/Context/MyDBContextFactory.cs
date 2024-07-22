using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hng.Infrastructure.Context
{
  public class MyDBContextFactory : IDesignTimeDbContextFactory<MyDBContext>
  {
    public MyDBContext CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var builder = new DbContextOptionsBuilder<MyDBContext>();
      var connectionString = configuration.GetConnectionString("DefaultConnection");

      builder.UseNpgsql(connectionString);

      return new MyDBContext(builder.Options);
    }
  }
}
