namespace MyFactory.WebApi.Contracts.Production;

public record ProductionCreateOrderRequest(
    Guid SpecificationId,
    int Quantity
);

