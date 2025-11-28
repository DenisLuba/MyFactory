namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsDeleteBomItemResponse(
    Guid SpecificationId,
    Guid BomItemId,
    SpecificationsStatus Status
);

