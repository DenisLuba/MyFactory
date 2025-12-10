using System;
using MyFactory.Domain.Entities.FinishedGoods;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

internal static class FinishedGoodsTestHelper
{
    public static Specification CreateSpecification(string sku)
    {
        return new Specification(sku, $"Specification {sku}", 10m, "Active", DateTime.UtcNow);
    }

    public static Warehouse CreateWarehouse(string name)
    {
        return new Warehouse(name, "Finished Goods", "Main");
    }

    public static Customer CreateCustomer(string name)
    {
        return new Customer(name, $"{name.ToLowerInvariant()}@example.com");
    }

    public static FinishedGoodsInventory CreateInventory(Specification specification, Warehouse warehouse, decimal quantity, decimal unitCost)
    {
        var inventory = new FinishedGoodsInventory(specification.Id, warehouse.Id);
        if (quantity > 0)
        {
            inventory.Receive(quantity, unitCost, DateOnly.FromDateTime(DateTime.UtcNow));
        }

        return inventory;
    }

    public static Shipment CreateShippedOrder(Customer customer, DateOnly shipmentDate, params (Guid SpecificationId, decimal Quantity, decimal UnitPrice)[] items)
    {
        var shipment = new Shipment($"SH-{Guid.NewGuid():N}"[..8], customer.Id, shipmentDate);

        foreach (var (specificationId, quantity, unitPrice) in items)
        {
            shipment.AddItem(specificationId, quantity, unitPrice);
        }

        shipment.Ship();

        return shipment;
    }
}
