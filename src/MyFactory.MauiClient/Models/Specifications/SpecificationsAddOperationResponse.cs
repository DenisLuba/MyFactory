namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsAddOperationResponse(
    Guid SpecificationId,
    SpecificationOperationItemResponse Item,
    SpecificationsStatus Status
);

