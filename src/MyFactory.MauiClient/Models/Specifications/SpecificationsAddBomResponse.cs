namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsAddBomResponse(
    Guid SpecificationId,
    SpecificationBomItemResponse Item,
    SpecificationsStatus Status
);

