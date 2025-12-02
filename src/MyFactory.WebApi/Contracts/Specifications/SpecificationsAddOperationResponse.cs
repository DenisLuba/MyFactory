namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddOperationResponse(
    Guid SpecificationId,
    SpecificationOperationItemResponse Item,
    SpecificationsStatus Status
);

