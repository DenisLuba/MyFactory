using System;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using Xunit;

namespace MyFactory.Domain.Tests.Specifications;

public class SpecificationTests
{
    [Fact]
    public void AddBomItem_WithValidData_AddsItem()
    {
        var specification = CreateSpecification();
        var materialId = Guid.NewGuid();

        var bomItem = specification.AddBomItem(materialId, 2.5m, "kg", 4.2m);

        Assert.Single(specification.BomItems);
        Assert.Equal(bomItem, specification.BomItems.Single());
        Assert.Equal(materialId, bomItem.MaterialId);
        Assert.Equal(2.5m, bomItem.Quantity);
        Assert.Equal(4.2m, bomItem.UnitCost);
    }

    [Fact]
    public void AddOperation_WithValidData_AddsOperation()
    {
        var specification = CreateSpecification();
        var operationId = Guid.NewGuid();
        var workshopId = Guid.NewGuid();

        var operation = specification.AddOperation(operationId, workshopId, 12.5m, 30m);

        Assert.Single(specification.Operations);
        Assert.Equal(operation, specification.Operations.Single());
        Assert.Equal(12.5m, operation.TimeMinutes);
        Assert.Equal(30m, operation.OperationCost);
    }

    [Fact]
    public void AddOperation_WithDuplicateCombination_Throws()
    {
        var specification = CreateSpecification();
        var operationId = Guid.NewGuid();
        var workshopId = Guid.NewGuid();
        specification.AddOperation(operationId, workshopId, 10m, 20m);

        Assert.Throws<DomainException>(() => specification.AddOperation(operationId, workshopId, 15m, 25m));
    }

    private static Specification CreateSpecification()
        => new("SKU-001", "Winter Jacket", 25m, "Draft", new DateTime(2025, 1, 1), "Warm jacket");
}
