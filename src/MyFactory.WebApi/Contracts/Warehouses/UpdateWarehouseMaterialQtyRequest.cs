namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record UpdateWarehouseMaterialQtyRequest(decimal QtyPerPackage, decimal? PackageCount = null);
