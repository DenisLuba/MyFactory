using System;

namespace MyFactory.WebApi.Contracts.Returns;

public record ReturnsListResponse(
    Guid ReturnId,
    string Customer,
    string ProductName,
    int Quantity,
    DateTime Date,
    string Reason,
    ReturnStatus Status
);
