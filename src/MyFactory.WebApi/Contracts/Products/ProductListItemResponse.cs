namespace MyFactory.WebApi.Contracts.Products;

public record ProductListItemResponse(
    Guid Id,
    string Sku,
    string Name,
    ProductStatus Status,
    string? Description,
    decimal? PlanPerHour,
    decimal? Version,
    decimal CostPrice);
