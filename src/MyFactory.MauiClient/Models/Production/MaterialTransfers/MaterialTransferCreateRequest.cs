namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferCreateRequest(
    DateTime Date,
    string ProductionOrder,
    string Warehouse,
    IReadOnlyList<MaterialTransferItemRequest> Items);
