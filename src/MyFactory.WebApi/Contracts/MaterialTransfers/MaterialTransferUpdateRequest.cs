namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferUpdateRequest(
    DateTime Date,
    string ProductionOrder,
    string Warehouse,
    IReadOnlyList<MaterialTransferItemRequest> Items);
