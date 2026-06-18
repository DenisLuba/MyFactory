namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record UpdateWarehouseProductQtyRequest(decimal QtyPerPackage, decimal? PackageCount = null);