using Microsoft.EntityFrameworkCore;
using Hng.Infrastructure.Context;
using Microsoft.Extensions.Logging;
using Hng.Domain.Entities;
using Bogus;

namespace Hng.Infrastructure.Services;
public class SeederService
{
    private readonly Dictionary<string, Guid> _entityIds;

    private readonly MyDBContext _dataContext;

    private readonly ILogger<SeederService> _logger;

    public SeederService(MyDBContext dataContext, ILogger<SeederService> logger)
    {
        _entityIds = [];
        _dataContext = dataContext;
        _logger = logger;
        PreGenerateGuids();
    }

    private void PreGenerateGuids()
    {
        _logger.LogDebug("Generating seed guids");
        _entityIds["User1"] = Guid.NewGuid();
        _entityIds["User2"] = Guid.NewGuid();
        _entityIds["Org1"] = Guid.NewGuid();
        _entityIds["Org2"] = Guid.NewGuid();
        _entityIds["Org3"] = Guid.NewGuid();
        _logger.LogDebug("Seed guids generated");
    }

    public async Task SeedUsers()
    {
        if (await _dataContext.Users.AnyAsync()) return;

        _logger.LogDebug("Inserting seed users");
        try
        {
            List<User> users =
            [
                CreateUser("User1"),
                CreateUser("User2")
            ];


            await _dataContext.AddRangeAsync(users);

            await _dataContext.SaveChangesAsync();

            _logger.LogDebug("Seed users inserted");
        }

        catch (Exception ex)
        {
            _logger.LogError("Error inserting seed users, {ex}", ex);
            // await transaction.RollbackAsync();
        }


    }

    public async Task SeedProfile()
    {
        if (await _dataContext.Profiles.AnyAsync()) return;

        _logger.LogDebug("Inserting seed profiles");

        try
        {
            var profiles = new List<Profile>
            {
                CreateProfile("User1"),
                CreateProfile("User2")
            };

            await _dataContext.Profiles.AddRangeAsync(profiles);

            await _dataContext.SaveChangesAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError("Error inserting seed profiles, {ex}", ex);
        }
    }

    public async Task SeedOrganisation()
    {
        if (await _dataContext.Organisations.AnyAsync()) return;

        _logger.LogDebug("Inserting seed organisations");

        try
        {

            List<Organisation> organisations =
            [
                CreateOrganisation("Org1"),
                CreateOrganisation("Org2"),
                CreateOrganisation("Org3")
            ];
            Console.WriteLine("Organisation list created");
            await _dataContext.AddRangeAsync(organisations);

            await _dataContext.SaveChangesAsync();

            _logger.LogDebug("Seed organisations inserted");
        }

        catch (Exception ex)
        {
            _logger.LogError("Error inserting seed organisations, {ex}", ex);
        }
    }

    public async Task SeedProducts()
    {
        if (await _dataContext.Products.AnyAsync()) return;

        _logger.LogDebug("Inserting seed products");

        try
        {
            List<Product> products =
            [
                CreateProduct("User1"),
                CreateProduct("User1"),
                CreateProduct("User2"),
                CreateProduct("User2"),
            ];


            await _dataContext.AddRangeAsync(products);
            await _dataContext.SaveChangesAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError("Error inserting seed products, {ex}", ex);
        }

    }

    public async Task SeedOrganisationUsers()
    {
        var set = _dataContext.Set<OrganisationUser>();
        if (set.Any()) return;

        _logger.LogDebug("Inserting seed organisation users");

        try
        {
            var organisationUsers = new List<OrganisationUser>
        {
            CreateOrganisationUser("User1", "Org1"),
            CreateOrganisationUser("User2", "Org2"),
            CreateOrganisationUser("User1", "Org3")
        };

            await set.AddRangeAsync(organisationUsers);
            await _dataContext.SaveChangesAsync();

            _logger.LogDebug("Seed organisation users inserted");
        }

        catch (Exception ex)
        {
            _logger.LogError("Error inserting seed organisation users, {ex}", ex);
        }

    }


    private User CreateUser(string userKey)
    {
        var user = new Faker<User>()
        .RuleFor(u => u.Id, _entityIds[userKey])
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber()).Generate();
        return user;
    }

    private Profile CreateProfile(string userKey)
    {
        var Profile = new Faker<Profile>()
         .RuleFor(p => p.FirstName, f => f.Name.FirstName())
         .RuleFor(p => p.LastName, f => f.Name.LastName())
         .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
         .RuleFor(p => p.AvatarUrl, f => f.Internet.Avatar())
         .RuleFor(p => p.UserId, _entityIds[userKey]).Generate();
        return Profile;
    }

    private Product CreateProduct(string userKey)
    {
        var product = new Faker<Product>()
       .RuleFor(p => p.Name, f => f.Commerce.ProductName())
       .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
       .RuleFor(p => p.UserId, _entityIds[userKey]).Generate();
        return product;
    }

    private Organisation CreateOrganisation(string orgKey)
    {
        var org = new Faker<Organisation>()
        .RuleFor(o => o.Id, _entityIds[orgKey])
        .RuleFor(o => o.Name, f => f.Company.CompanyName())
        .RuleFor(o => o.Description, f => f.Company.CatchPhrase()).Generate();
        return org;
    }

    private OrganisationUser CreateOrganisationUser(string userKey, string organisationKey)
    {

        var organisationUser = new Faker<OrganisationUser>().RuleFor(o => o.UserId, _entityIds[userKey])
                    .RuleFor(o => o.OrganisationId, _entityIds[organisationKey]).Generate();
        return organisationUser;
    }

}
