namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record AddMaterialToWarehouseRequest(Guid MaterialId, decimal Qty);
