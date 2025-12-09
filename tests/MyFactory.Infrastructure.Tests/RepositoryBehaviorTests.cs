using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;
using MyFactory.Infrastructure.Repositories;
using MyFactory.Infrastructure.Repositories.Specifications;
using Xunit;

namespace MyFactory.Infrastructure.Tests;

public class RepositoryBehaviorTests
{
    [Fact]
    public async Task RemoveAsync_ShouldMarkEntityAsDeletedAndRespectFilters()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var repository = new EfRepository<Material>(context);

        var materialType = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(materialType);
        var material = new Material("Steel Plate", materialType.Id, "kg");
        await repository.AddAsync(material);
        await context.SaveChangesAsync();

        await repository.RemoveAsync(material);
        await context.SaveChangesAsync();

        var visible = repository.AsQueryable().ToList();
        var withDeleted = repository.AsQueryable(includeSoftDeleted: true).ToList();

        visible.Should().BeEmpty();
        withDeleted.Should().ContainSingle();
        withDeleted.First().IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Specification_ShouldLoadRelatedGraph()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var specification = new Specification("SKU-10", "Widget", 12.5m, "Active", DateTime.UtcNow.AddDays(-10));
        await context.Specifications.AddAsync(specification);
        var workshop = new Workshop("Assembly", "General");
        await context.Workshops.AddAsync(workshop);
        var order = ProductionOrder.Create("PO-1001", specification.Id, 500, DateTime.UtcNow.AddDays(-2));
        order.ScheduleStage(workshop.Id, "Assembly");
        await context.ProductionOrders.AddAsync(order);
        await context.SaveChangesAsync();

        var orderRepository = new EfRepository<ProductionOrder>(context);
        var spec = new ProductionOrderWithDetailsSpecification(order.Id);
        var loaded = await orderRepository.FirstOrDefaultAsync(spec);

        loaded.Should().NotBeNull();
        loaded!.Stages.Should().HaveCount(1);
    }
}
