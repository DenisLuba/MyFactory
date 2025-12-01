namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferUpdateRequest(
    DateTime Date,
    string ProductionOrder,
    string Warehouse,
    IReadOnlyList<MaterialTransferItemRequest> Items);
