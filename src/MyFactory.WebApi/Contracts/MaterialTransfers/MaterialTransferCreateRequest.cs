namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferCreateRequest(
    DateTime Date,
    string ProductionOrder,
    string Warehouse,
    IReadOnlyList<MaterialTransferItemRequest> Items);
