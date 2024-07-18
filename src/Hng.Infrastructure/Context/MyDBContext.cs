using Hng.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Context
{
    public class MyDBContext : IdentityDbContext<User, IdentityRole<int>, int>

    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<OrganisationUser> OrganisationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
