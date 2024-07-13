using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Context
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        { }

            public DbSet<Role> Roles { get; set; }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Permission> Permissions { get; set; }
    } 
}

 
