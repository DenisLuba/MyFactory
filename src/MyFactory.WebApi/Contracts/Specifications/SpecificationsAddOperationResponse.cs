namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsAddOperationResponse(
    Guid SpecificationId,
    Guid OperationId,
    SpecificationsStatus Status
);

