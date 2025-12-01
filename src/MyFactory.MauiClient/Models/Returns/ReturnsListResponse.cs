using System;

namespace MyFactory.MauiClient.Models.Returns;

public record ReturnsListResponse(
    Guid ReturnId,
    string Customer,
    string ProductName,
    int Quantity,
    DateTime Date,
    string Reason,
    ReturnStatus Status
);
