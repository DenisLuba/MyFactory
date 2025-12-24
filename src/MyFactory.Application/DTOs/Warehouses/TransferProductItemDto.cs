namespace MyFactory.Application.DTOs.Warehouses;

public sealed record TransferProductItemDto(
    Guid ProductId,
    int Qty
);