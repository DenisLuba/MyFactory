namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsGetResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    string? Description,
    SpecificationsStatus Status,
    int ImagesCount
);

