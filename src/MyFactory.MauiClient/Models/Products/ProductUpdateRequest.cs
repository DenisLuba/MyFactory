namespace MyFactory.MauiClient.Models.Products;

public record ProductUpdateRequest(
    string Sku,
    string Name,
    double PlanPerHour,
    string Description
);
