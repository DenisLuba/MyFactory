using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Models.Products;

public record UpdateProductRequest(
    string Name,
    int? PlanPerHour,
    ProductStatus Status,
    string? Description,
    int? Version);
