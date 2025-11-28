namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsAddBomResponse(
    Guid SpecificationId,
    Guid BomItemId,
    SpecificationsStatus Status
);

