namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record AddMaterialToWarehouseRequest(Guid MaterialId, decimal QtyPerPackage, decimal? PackageCount = null);
