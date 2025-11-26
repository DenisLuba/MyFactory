namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddOperationRequest(
    string Code,
    string Name,
    double Minutes,
    decimal Cost
);

