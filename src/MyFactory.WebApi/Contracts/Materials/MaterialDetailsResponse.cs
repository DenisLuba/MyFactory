namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialDetailsResponse(
    Guid Id,
    string Name,
    string MaterialType,
    string UnitCode,
    string? Color,
    decimal TotalQty,
    IReadOnlyList<WarehouseQtyResponse> Warehouses,
    IReadOnlyList<MaterialPurchaseHistoryItemResponse> PurchaseHistory);
