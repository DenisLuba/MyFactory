namespace MyFactory.Application.DTOs.Warehouses;

public sealed record TransferProductItemDto(
    Guid ProductId,
    decimal QtyPerPackage,
    decimal? PackageCount = null
);