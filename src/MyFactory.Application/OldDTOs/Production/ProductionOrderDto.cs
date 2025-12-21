using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.OldDTOs.Production;

public sealed record ProductionOrderDto(
    Guid Id,
    string OrderNumber,
    Guid SpecificationId,
    string SpecificationName,
    decimal QuantityOrdered,
    string Status,
    DateTime CreatedAt,
    IReadOnlyCollection<ProductionOrderAllocationDto> Allocations,
    IReadOnlyCollection<ProductionStageDto> Stages)
{
    public static ProductionOrderDto FromEntity(
        ProductionOrder order,
        IReadOnlyDictionary<Guid, Specification> specifications,
        IReadOnlyDictionary<Guid, Workshop> workshops,
        IReadOnlyDictionary<Guid, Employee> employees)
    {
        var specificationName = specifications.TryGetValue(order.SpecificationId, out var specification)
            ? specification.Name
            : string.Empty;

        var allocationDtos = order.Allocations
            .Select(allocation => ProductionOrderAllocationDto.FromEntity(allocation, workshops))
            .ToList();

        var stageDtos = order.Stages
            .OrderBy(stage => stage.RecordedAt ?? stage.StartedAt ?? DateTime.MaxValue)
            .Select(stage => ProductionStageDto.FromEntity(stage, workshops, employees))
            .ToList();

        return new ProductionOrderDto(
            order.Id,
            order.OrderNumber,
            order.SpecificationId,
            specificationName,
            order.QuantityOrdered,
            order.Status,
            order.CreatedAt,
            allocationDtos,
            stageDtos);
    }
}
