using System;
using MyFactory.MauiClient.Models.Shipments;

namespace MyFactory.MauiClient.UIModels.FinishedGoods;

public record ShipmentListItem(
    Guid ShipmentId,
    string Customer,
    string ProductName,
    int Quantity,
    DateTime Date,
    decimal TotalAmount,
    ShipmentStatus Status
);
