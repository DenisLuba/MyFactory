namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record TransferProductItemRequest(Guid ProductId, decimal QtyPerPackage, decimal? PackageCount = null);
