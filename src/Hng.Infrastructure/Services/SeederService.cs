using Bogus;
using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hng.Infrastructure.Services;
public class SeederService
{
    private readonly Dictionary<string, Guid> _entityIds;

    private readonly ApplicationDbContext _dataContext;

    private readonly ILogger<SeederService> _logger;

    public SeederService(ApplicationDbContext dataContext, ILogger<SeederService> logger)
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
        if (await _dataContext.Organizations.AnyAsync()) return;

        _logger.LogDebug("Inserting seed organisations");

        try
        {

            List<Organization> organisations =
            [
                CreateOrganisation(),
                CreateOrganisation(),
                CreateOrganisation()
            ];
            Console.WriteLine("Organization list created");
            await _dataContext.AddRangeAsync(organisations);

            await _dataContext.SaveChangesAsync();

            _logger.LogDebug("Seed organizations inserted");
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

    public async Task SeedCategory()
    {
        if (await _dataContext.Categories.AnyAsync()) return;

        _logger.LogDebug("Inserting seed categories");

        try
        {

            List<Category> categories =
            [
                CreateCategory(),
                CreateCategory(),
                CreateCategory()
            ];

            Console.WriteLine("Category list created");
            await _dataContext.AddRangeAsync(categories);
            await _dataContext.SaveChangesAsync();

            _logger.LogDebug("Seed categories inserted");
        }

        catch (Exception ex)
        {
            _logger.LogError("Error inserting seed categories, {ex}", ex);
        }
    }

    private User CreateUser(string userKey)
    {
        var organizations = new List<Organization>
            {
                CreateOrganisation(),
                CreateOrganisation(),
                CreateOrganisation()
            };
        var user = new Faker<User>()
        .RuleFor(u => u.Id, _entityIds[userKey])
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())

        .RuleFor(u => u.Organizations, f => organizations)

        .Generate();
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
       .RuleFor(p => p.Id, Guid.NewGuid())
       .RuleFor(p => p.Name, f => f.Commerce.ProductName())
       .RuleFor(p => p.Price, f => f.Finance.Amount())
       .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
       .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
       .RuleFor(p => p.UserId, _entityIds[userKey]).Generate();
        return product;
    }

    private Organization CreateOrganisation()
    {
        var org = new Faker<Organization>()
        .RuleFor(o => o.Id, Guid.NewGuid())
        .RuleFor(o => o.Name, f => f.Company.CompanyName())
        .RuleFor(o => o.Description, f => f.Company.CatchPhrase()).Generate();
        return org;
    }

    private Category CreateCategory()
    {
        var category = new Faker<Category>()
        .RuleFor(o => o.Id, Guid.NewGuid())
        .RuleFor(o => o.Name, f => f.Commerce.Categories(1).First())
        .RuleFor(o => o.Slug, f => f.Name.Suffix())
        .RuleFor(o => o.Description, f => f.Company.CatchPhrase()).Generate();

        return category;
    }
}
