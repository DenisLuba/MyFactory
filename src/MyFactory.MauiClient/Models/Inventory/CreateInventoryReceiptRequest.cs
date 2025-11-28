namespace MyFactory.MauiClient.Models.Inventory;

public record CreateInventoryReceiptRequest(
    Guid WarehouseId,
    DateTime ReceiptDate,
    string ReferenceNumber,
    List<ReceiptItem> Items);
