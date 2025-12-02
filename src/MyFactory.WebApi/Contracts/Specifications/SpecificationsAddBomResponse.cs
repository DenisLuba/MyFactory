namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddBomResponse(
    Guid SpecificationId,
    SpecificationBomItemResponse Item,
    SpecificationsStatus Status
);

