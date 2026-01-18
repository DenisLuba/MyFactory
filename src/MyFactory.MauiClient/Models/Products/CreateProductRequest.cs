using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Models.Products;

public record CreateProductRequest(
    string Sku,
    string Name,
    ProductStatus Status,
    int? PlanPerHour,
    string? Description,
    int? Version);
