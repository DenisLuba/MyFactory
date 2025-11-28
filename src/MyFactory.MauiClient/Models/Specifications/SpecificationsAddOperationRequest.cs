namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsAddOperationRequest(
    string Code,
    string Name,
    double Minutes,
    decimal Cost
);

