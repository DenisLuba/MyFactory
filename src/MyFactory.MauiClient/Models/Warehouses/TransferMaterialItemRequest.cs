namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record TransferMaterialItemRequest(Guid MaterialId, decimal QtyPerPackage, decimal? PackageCount = null);
