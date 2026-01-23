namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record AddProductToWarehouseRequest(Guid ProductId, int Qty);
