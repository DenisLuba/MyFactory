using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Models.Products;

public record UpdateProductRequest(
    string Name,
    decimal? PlanPerHour,
    ProductStatus Status,
    string? Description,
    decimal? Version);
