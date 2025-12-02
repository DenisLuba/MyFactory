using System;

namespace MyFactory.MauiClient.UIModels.Warehouse;

public record MaterialReceiptLineItem(
    Guid Id,
    Guid MaterialId,
    string Material,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal TotalAmount
);