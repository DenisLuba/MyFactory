using System;

namespace MyFactory.Application.OldDTOs.Specifications;

public sealed record SpecificationCostDto(
    Guid SpecificationId,
    DateOnly AsOfDate,
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost);
