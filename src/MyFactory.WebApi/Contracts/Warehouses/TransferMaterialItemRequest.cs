namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record TransferMaterialItemRequest(Guid MaterialId, decimal QtyPerPackage, decimal? PackageCount = null);
