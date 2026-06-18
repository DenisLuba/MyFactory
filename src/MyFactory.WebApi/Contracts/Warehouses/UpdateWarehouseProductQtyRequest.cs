namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record UpdateWarehouseProductQtyRequest(decimal QtyPerPackage, decimal? PackageCount = null);