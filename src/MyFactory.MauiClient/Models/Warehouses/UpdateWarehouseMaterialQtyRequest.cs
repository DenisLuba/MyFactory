namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record UpdateWarehouseMaterialQtyRequest(decimal QtyPerPackage, decimal? PackageCount = null);
