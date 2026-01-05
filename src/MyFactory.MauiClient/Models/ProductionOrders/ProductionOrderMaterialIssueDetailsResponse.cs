namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderMaterialIssueDetailsResponse(
    ProductionOrderMaterialResponse Material,
    IReadOnlyList<ProductionOrderMaterialWarehouseResponse> Warehouses);
