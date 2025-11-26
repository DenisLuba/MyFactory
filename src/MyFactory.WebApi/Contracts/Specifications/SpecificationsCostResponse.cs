namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsCostResponse(
    Guid SpecificationId,
    DateTime AsOf,
    decimal MaterialsCost,
    decimal OperationsCost,
    decimal WorkshopExpenses,
    decimal TotalCost
);

