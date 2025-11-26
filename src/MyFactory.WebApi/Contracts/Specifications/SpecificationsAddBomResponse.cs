namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddBomResponse(
    Guid SpecificationId,
    Guid BomItemId,
    SpecificationsStatus Status
);

