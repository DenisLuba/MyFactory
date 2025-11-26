namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsUploadImageResponse(
    Guid SpecificationId,
    SpecificationsStatus Status
);

