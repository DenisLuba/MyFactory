namespace MyFactory.WebApi.Contracts.Inventory;

public record CreateInventoryReceiptRequest(
    Guid WarehouseId,
    DateTime ReceiptDate,
    string ReferenceNumber,
    List<ReceiptItem> Items
);

public record ReceiptItem(
    Guid MaterialId,
    double Quantity,
    decimal UnitPrice,
    string? BatchNumber = null
);
