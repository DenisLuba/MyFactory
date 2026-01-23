namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record AddProductToWarehouseRequest(Guid ProductId, int Qty);
