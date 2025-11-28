namespace MyFactory.MauiClient.Models.Inventory;

public record CreateInventoryReceiptResponse(
    Guid ReceiptId,
    StatusInventory Status);
