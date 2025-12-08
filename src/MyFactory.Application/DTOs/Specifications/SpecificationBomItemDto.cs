using System;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationBomItemDto(
    Guid Id,
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal? UnitCost,
    decimal? Cost)
{
    public static SpecificationBomItemDto FromEntity(SpecificationBomItem item, string materialName)
    {
        var unitCost = item.UnitCost;
        decimal? cost = unitCost.HasValue ? item.Quantity * unitCost.Value : null;
        return new SpecificationBomItemDto(
            item.Id,
            item.MaterialId,
            materialName,
            item.Quantity,
            item.Unit,
            unitCost,
            cost);
    }
}
