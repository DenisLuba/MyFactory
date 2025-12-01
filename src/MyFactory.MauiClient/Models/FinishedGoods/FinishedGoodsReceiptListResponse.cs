using System;

namespace MyFactory.MauiClient.Models.FinishedGoods;

public record FinishedGoodsReceiptListResponse(
    Guid ReceiptId,
    string ProductName,
    int Quantity,
    DateTime Date,
    string Warehouse,
    decimal UnitPrice,
    decimal Sum
);
