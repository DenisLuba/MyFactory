namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsUpdateRequest(
    string Sku,
    string Name,
    double PlanPerHour,
    string? Description
);

