using System;

namespace MyFactory.WebApi.Contracts.Returns;

public record ReturnCardResponse(
    Guid ReturnId,
    string Customer,
    string ProductName,
    int Quantity,
    DateTime Date,
    string Reason,
    ReturnStatus Status,
    string? Comment
);
