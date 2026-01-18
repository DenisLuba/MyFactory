namespace MyFactory.MauiClient.Models.Products;

public record ProductListItemResponse(
    Guid Id,
    string Sku,
    string Name,
    ProductStatus Status,
    string? Description,
    int? PlanPerHour,
    int? Version,
    decimal CostPrice);
