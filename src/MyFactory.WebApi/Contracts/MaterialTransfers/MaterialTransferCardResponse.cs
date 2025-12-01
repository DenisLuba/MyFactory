namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferCardResponse(
    Guid TransferId,
    string DocumentNumber,
    DateTime Date,
    string ProductionOrder,
    string Warehouse,
    decimal TotalAmount,
    MaterialTransferStatus Status,
    IReadOnlyList<MaterialTransferItemDto> Items);
