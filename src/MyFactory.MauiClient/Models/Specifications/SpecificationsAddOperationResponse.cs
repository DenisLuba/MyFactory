namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsAddOperationResponse(
    Guid SpecificationId,
    Guid OperationId,
    SpecificationsStatus Status
);

