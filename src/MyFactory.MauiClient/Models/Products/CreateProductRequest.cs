namespace MyFactory.MauiClient.Models.Products;

public record CreateProductRequest(
    string Name,
    ProductStatus Status,
    decimal? PlanPerHour,
    string? Description,
    decimal? Version);
