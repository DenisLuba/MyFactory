using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Infrastructure.Persistence;
using MyFactory.Infrastructure.Repositories;
using Xunit;

namespace MyFactory.Infrastructure.Tests;

public class MaterialRepositoryTests
{
    [Fact]
    public async Task AddPriceHistoryAndRetrieve_ShouldReturnPriceHistory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ApplicationDbContext(options);
        var repository = new MaterialRepository(context);

        var materialType = new MaterialType("Metals");
        var supplier = new Supplier("Seed Supplier", "seed@example.com");
        await context.MaterialTypes.AddAsync(materialType);
        await context.Suppliers.AddAsync(supplier);

        var material = new Material("Steel Bar", materialType.Id, "kg");
        await repository.AddAsync(material);
        await context.SaveChangesAsync();

        var entry = material.AddPrice(supplier.Id, 22.5m, DateTime.UtcNow.Date, "TEST-001");
        await repository.AddPriceHistoryAsync(entry);
        await context.SaveChangesAsync();

        var withHistory = await repository.GetByIdWithHistoryAsync(material.Id);

        withHistory.Should().NotBeNull();
        withHistory!.PriceHistory.Should().HaveCount(1);
        withHistory.PriceHistory.First().Price.Should().Be(22.5m);
    }
}
