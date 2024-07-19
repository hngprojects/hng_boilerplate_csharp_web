using Hng.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Context
{
    public class MyDBContext(DbContextOptions options) : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>(options)
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Profile> Profiles {  get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<UserOrganisation> UserOrganisations { get; set; }
        public DbSet<Product> Products {  get; set; }
    }
}
