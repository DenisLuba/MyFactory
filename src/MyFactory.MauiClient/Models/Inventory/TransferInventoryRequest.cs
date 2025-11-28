namespace MyFactory.MauiClient.Models.Inventory;

public record TransferInventoryRequest(
    Guid MaterialId,
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    double Quantity,
    string Reason,
    DateTime TransferDate);
