namespace MyFactory.MauiClient.Models.Products;

public record ProductDetailsResponse(
    Guid Id,
    string Name,
    decimal? PlanPerHour,
    decimal MaterialsCost,
    decimal ProductionCost,
    decimal TotalCost,
    IReadOnlyList<ProductBomItemResponse> Bom,
    IReadOnlyList<ProductDepartmentCostResponse> ProductionCosts,
    IReadOnlyList<ProductAvailabilityResponse> Availability);
