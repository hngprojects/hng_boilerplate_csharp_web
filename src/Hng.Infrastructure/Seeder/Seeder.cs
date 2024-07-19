using Hng.Domain.Entities.Models;
using Hng.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Seeder
{
    public static class Seeder
    {

        public static async Task Seed(MyDBContext context, UserManager<User> usermanager)
        {
            await SeedOrganization(context);
            await SeedUser(usermanager);
            await SeedUserOrganisation(context);
            await SeedUserProfile(context);
        }

        private async static Task SeedOrganization(MyDBContext context)
        {
           
            if (!await context.Organisations.AnyAsync())
            {
                List<Organisation> organisations = new List<Organisation>()
            {
                 new Organisation()
                 {
                      Name = "Organization A",
                      Description = "Sample Description"
                 },
                 new Organisation()
                 {
                      Name = "Organization B",
                      Description = "Sample Description"
                 }
            };
                await context.AddRangeAsync(organisations);
                await context.SaveChangesAsync();
            }
        }

        private async static Task SeedUser(UserManager<User> userManager)
        {
            var users = await userManager.Users.AnyAsync();
            if (!users)
            {
                List<User> newUsers = new List<User>()
                {
                   new User(){
                        Email= "johndoe@example.com",
                        UserName = "johndoe@example.com"
                   },
                   new User(){
                        Email= "janedoe@example.com",
                        UserName= "janedoe@example.com",
                   }
                };

                foreach (var user in newUsers)
                {
                    var createUser = await userManager.CreateAsync(user, "Password");
                    if (!createUser.Succeeded) break;
                }

            };
        }

        private async static Task SeedUserProfile(MyDBContext context)
        {
            var userMapped = await context.Profiles.AnyAsync();
            if (userMapped) return;
            var users = await context.Users.ToListAsync();
            List<Profile> userOrganisations = new();
            for (int i = 0; i < users.Count; i++)
            {
                userOrganisations.Add(new Profile()
                {
                    FirstName = "User",
                    LastName ="User",
                    UserId = users[i].Id
                });
            }
            await context.AddRangeAsync(userOrganisations);
            await context.SaveChangesAsync();
        }

        private async static Task SeedUserOrganisation(MyDBContext context)
        {
            var userMapped = await context.UserOrganisations.AnyAsync();
            if (userMapped) return;
            var users = await context.Users.ToListAsync();
            var organisations = await context.Organisations.ToListAsync();
            List<UserOrganisation> userOrganisations = new();
            for (int i = 0; i < users.Count; i++)
            {
                userOrganisations.Add(new UserOrganisation()
                {
                    OrganisationId = organisations[i].Id,
                    UserId = users[i].Id
                });
            }
            await context.AddRangeAsync(userOrganisations);
            await context.SaveChangesAsync();
        }
    }

  
}
