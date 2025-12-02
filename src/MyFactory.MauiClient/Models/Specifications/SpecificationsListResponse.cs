namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationsListResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    SpecificationsStatus Status,
    int ImagesCount
);
