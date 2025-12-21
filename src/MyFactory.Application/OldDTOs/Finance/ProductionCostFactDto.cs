using System;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.OldDTOs.Finance;

public sealed record ProductionCostFactDto(
    Guid Id,
    Guid SpecificationId,
    string SpecificationName,
    decimal QuantityProduced,
    decimal MaterialCost,
    decimal LaborCost,
    decimal OverheadCost,
    decimal TotalCost)
{
    public static ProductionCostFactDto FromEntity(ProductionCostFact fact, string specificationName)
    {
        return new ProductionCostFactDto(
            fact.Id,
            fact.SpecificationId,
            specificationName,
            fact.QuantityProduced,
            fact.MaterialCost,
            fact.LaborCost,
            fact.OverheadCost,
            fact.TotalCost);
    }
}
