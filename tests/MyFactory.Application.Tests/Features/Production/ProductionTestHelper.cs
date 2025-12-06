using System;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.Tests.Features.Production;

internal static class ProductionTestHelper
{
    public static Specification CreateSpecification(string sku)
    {
        return new Specification(sku, $"Specification {sku}", 10, "Active", DateTime.UtcNow);
    }

    public static Workshop CreateWorkshop(string name)
    {
        return new Workshop(name, "Default");
    }

    public static Employee CreateEmployee(string name)
    {
        return new Employee(name, "Operator", 1, 10, 0);
    }

    public static ProductionOrder CreateProductionOrder(Specification specification, decimal quantity)
    {
        return ProductionOrder.Create($"PO-{Guid.NewGuid():N}"[..8], specification.Id, quantity, DateTime.UtcNow);
    }
}
