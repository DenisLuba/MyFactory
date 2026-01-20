namespace MyFactory.MauiClient.Models.Products;

public record ProductDetailsResponse(
    Guid Id,
    string Sku,
    string Name,
    decimal? PlanPerHour,
    string? Description,
    decimal? Version,
    ProductStatus Status,
    decimal MaterialsCost,
    decimal ProductionCost,
    decimal TotalCost,
    IReadOnlyList<ProductBomItemResponse> Bom,
    IReadOnlyList<ProductDepartmentCostResponse> ProductionCosts,
    IReadOnlyList<ProductAvailabilityResponse> Availability);
