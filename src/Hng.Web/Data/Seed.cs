using Hng.Domain.Models;
using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hng.Web.Data
{
    public class Seed
    {
        public static async Task SeedOrganisation(MyDBContext dataContext)
        {
            if (await dataContext.Organisations.AnyAsync()) return;
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/Seeder/OrganisationSeeder.json");
            var organisations = JsonConvert.DeserializeObject<List<Organisation>>(dataFronJsonFile);
            if (organisations == null) return;

            foreach (var organisation in organisations)
            {
                var orgId = Guid.NewGuid().ToString();
                dataContext.Organisations.Add(new Organisation
                {
                    Name = organisation.Name,
                    // OrgId = orgId,
                    Description = organisation.Description,
                });
            }

            await dataContext.SaveChangesAsync();
        }

        public static async Task SeedProducts(MyDBContext dataContext)
        {
            if (await dataContext.Products.AnyAsync()) return;
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/Seeder/ProductSeeder.json");
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
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/Seeder/OrganisationUser.json");
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

        public static async Task SeedProfile(MyDBContext dataContext)
        {
            if (await dataContext.Profiles.AnyAsync()) return;
            var dataFronJsonFile = await System.IO.File.ReadAllTextAsync("Data/Seeder/ProfileSeeder.json");
            var profiles = JsonConvert.DeserializeObject<List<Profile>>(dataFronJsonFile);
            if (profiles == null) return;

            foreach (var profile in profiles)
            {
                dataContext.Profiles.Add(new Profile
                {
                    UserId = profile.UserId,
                    PhoneNumber = profile.PhoneNumber,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    AvatarUrl = profile.AvatarUrl
                });
            }

            await dataContext.SaveChangesAsync();
        }

        public static async Task SeedUsers(MyDBContext myDBContext)
        {
            if (await myDBContext.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/Seeder/UserSeeder.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            if (users == null) return;

            foreach (var user in users)
            {
                await myDBContext.Users.AddAsync(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    AvatarUrl = user.AvatarUrl
                });
            }

            await myDBContext.SaveChangesAsync();
        }
    }
}