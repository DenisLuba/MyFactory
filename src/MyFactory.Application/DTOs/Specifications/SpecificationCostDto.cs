using System;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationCostDto(
    Guid SpecificationId,
    DateTime AsOfDate,
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost);
