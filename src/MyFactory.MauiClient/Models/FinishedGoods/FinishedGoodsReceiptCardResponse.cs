using System;

namespace MyFactory.MauiClient.Models.FinishedGoods;

public record FinishedGoodsReceiptCardResponse(
    Guid ReceiptId,
    string DocumentNumber,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Sum,
    DateTime Date,
    string Warehouse,
    FinishedGoodsStatus Status
);
