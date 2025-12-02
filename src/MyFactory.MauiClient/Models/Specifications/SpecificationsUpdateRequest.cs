namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsUpdateRequest(
    string Sku,
    string Name,
    double PlanPerHour,
    string? Description
);

