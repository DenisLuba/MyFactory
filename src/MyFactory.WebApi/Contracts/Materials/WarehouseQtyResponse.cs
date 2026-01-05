namespace MyFactory.WebApi.Contracts.Materials;

public record WarehouseQtyResponse(
    Guid WarehouseId,
    string WarehouseName,
    decimal Qty,
    string UnitCode);
