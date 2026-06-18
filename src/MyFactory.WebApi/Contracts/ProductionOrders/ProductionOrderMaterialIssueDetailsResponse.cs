namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderMaterialIssueDetailsResponse(
    ProductionOrderMaterialResponse Material,
    IReadOnlyList<ProductionOrderMaterialWarehouseResponse> Warehouses);
