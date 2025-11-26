namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsDeleteBomItemResponse(
    Guid SpecificationId,
    Guid BomItemId,
    SpecificationsStatus Status
);

