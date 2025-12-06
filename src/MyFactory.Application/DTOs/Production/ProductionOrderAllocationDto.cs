using System;
using System.Collections.Generic;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.DTOs.Production;

public sealed record ProductionOrderAllocationDto(
    Guid Id,
    Guid WorkshopId,
    string WorkshopName,
    decimal QuantityAllocated)
{
    public static ProductionOrderAllocationDto FromEntity(
        ProductionOrderAllocation allocation,
        IReadOnlyDictionary<Guid, Workshop> workshops)
    {
        var workshopName = workshops.TryGetValue(allocation.WorkshopId, out var workshop)
            ? workshop.Name
            : string.Empty;

        return new ProductionOrderAllocationDto(
            allocation.Id,
            allocation.WorkshopId,
            workshopName,
            allocation.QuantityAllocated);
    }
}
