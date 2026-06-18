namespace MyFactory.MauiClient.Models.Products;

public record CreateProductRequest(
    string Name,
    Guid? ProductTypeId,
    ProductStatus Status,
    decimal? PlanPerHour,
    string? Description,
    decimal? Version);
