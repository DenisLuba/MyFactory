namespace MyFactory.WebApi.Contracts.Products;

public record ProductDetailsResponse(
    Guid Id,
    string Sku,
    string Name,
    int? PlanPerHour,
    string? Description,
    int? Version,
    ProductStatus Status,
    decimal MaterialsCost,
    decimal ProductionCost,
    decimal TotalCost,
    IReadOnlyList<ProductBomItemResponse> Bom,
    IReadOnlyList<ProductDepartmentCostResponse> ProductionCosts,
    IReadOnlyList<ProductAvailabilityResponse> Availability);
