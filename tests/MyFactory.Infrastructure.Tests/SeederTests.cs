using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MyFactory.Infrastructure.Common;
using MyFactory.Infrastructure.Persistence.Seeds;
using Xunit;

namespace MyFactory.Infrastructure.Tests;

public class SeederTests
{
    [Fact]
    public async Task SeedAsync_ShouldPopulateReferenceData()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var settings = Options.Create(new Settings { SeedDemoData = true });
        var seeder = new InitialDataSeeder(context, settings, NullLogger<InitialDataSeeder>.Instance);

        await seeder.SeedAsync();

        (await context.Roles.CountAsync()).Should().BeGreaterThan(0);
        (await context.Suppliers.CountAsync()).Should().BeGreaterThan(0);
        (await context.MaterialTypes.CountAsync()).Should().BeGreaterThan(0);
        (await context.Materials.CountAsync()).Should().BeGreaterThan(0);
        (await context.Warehouses.CountAsync()).Should().BeGreaterThan(0);
        (await context.Workshops.CountAsync()).Should().BeGreaterThan(0);
    }
}
