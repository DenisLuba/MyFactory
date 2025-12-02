namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsCreateRequest(
    string Sku,
    string Name,
    double PlanPerHour,
    string? Description
);

