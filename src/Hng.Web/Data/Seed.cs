using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Domain.Models;
using Hng.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hng.Web.Data
{
    public class Seed
    {
         public static async Task SeedOrganisation(MyDBContext dataContext)
        {
            if (await dataContext.Organisations.AnyAsync()) return; 
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/OrganisationSeeder.json");
            var organisations = JsonConvert.DeserializeObject<List<Organisation>>(dataFronJsonFile);
            if (organisations == null) return;

            foreach (var organisation in organisations)
            {
                var orgId = Guid.NewGuid().ToString();
                dataContext.Organisations.Add(new Organisation
               {
                    Name = organisation.Name,
                    OrgId = orgId,
                    Description = organisation.Description,
               });
            }

            await dataContext.SaveChangesAsync();
        }

          public static async Task SeedProducts(MyDBContext dataContext)
        {
            if (await dataContext.Products.AnyAsync()) return; 
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/ProductSeeder.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(dataFronJsonFile);
            if (products == null) return;

            foreach (var product in products)
            {
                dataContext.Products.Add(new Product
               {
                    Name = product.Name,
                    Description = product.Description,
                    UserId = product.UserId
               });
            }

            await dataContext.SaveChangesAsync();
        }


         public static async Task SeedOrganisationUser(MyDBContext dataContext)
        {
            if (await dataContext.OrganisationUsers.AnyAsync()) return; 
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/OrganisationUser.json");
            var organisationUsers = JsonConvert.DeserializeObject<List<OrganisationUser>>(dataFronJsonFile);
            if (organisationUsers == null) return;

            foreach (var organisationUser in organisationUsers)
            {
                dataContext.OrganisationUsers.Add(new OrganisationUser
               {
                   OrganisationId = organisationUser.OrganisationId,
                   UserId = organisationUser.UserId
               });
            }

            await dataContext.SaveChangesAsync();
        }

         public static async Task SeedUsers(UserManager<User> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeeder.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            if (users == null) return;

            foreach (var user in users)
            {
                user.UserName = user.Email.ToLower();
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user,"Password10*");

                if (result.Succeeded)
                {
                   Console.WriteLine("User created successfuly");
                }
            }
        }
    }
}