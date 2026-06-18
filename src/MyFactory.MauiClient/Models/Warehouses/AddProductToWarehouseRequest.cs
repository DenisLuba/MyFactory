namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record AddProductToWarehouseRequest(Guid ProductId, decimal QtyPerPackage, decimal? PackageCount = null);
