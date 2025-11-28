namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsCostResponse(
    Guid SpecificationId,
    DateTime AsOfDate,
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost
);

