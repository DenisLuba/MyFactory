namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsAddOperationRequest(
    Guid OperationId,
    double Minutes,
    decimal Cost
);

