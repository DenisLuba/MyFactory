namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddOperationRequest(
    Guid OperationId,
    double Minutes,
    decimal Cost
);

