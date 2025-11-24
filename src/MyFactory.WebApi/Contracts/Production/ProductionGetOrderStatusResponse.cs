namespace MyFactory.WebApi.Contracts.Production;

public record ProductionGetOrderStatusResponse(
    Guid Id,
    ProductionStatus Status
);
