namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferListResponse(
    Guid TransferId,
    string DocumentNumber,
    DateTime Date,
    string ProductionOrder,
    string Warehouse,
    decimal TotalAmount,
    MaterialTransferStatus Status);
