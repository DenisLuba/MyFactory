using System;

namespace MyFactory.WebApi.Contracts.FinishedGoods;

public record FinishedGoodsReceiptListResponse(
    Guid ReceiptId,
    string ProductName,
    int Quantity,
    DateTime Date,
    string Warehouse,
    decimal UnitPrice,
    decimal Sum
);
