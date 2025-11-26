namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsUpdateResponse(
    Guid Id,
    SpecificationsStatus Status
);

