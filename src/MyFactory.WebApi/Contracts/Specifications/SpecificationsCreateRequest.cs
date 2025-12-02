namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationsCreateRequest(
    string Sku,
    string Name,
    double PlanPerHour,
    string? Description
);

