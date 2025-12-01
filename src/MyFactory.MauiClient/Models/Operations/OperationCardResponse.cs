using System;

namespace MyFactory.MauiClient.Models.Operations;

public record OperationCardResponse(
    Guid Id,
    string Code,
    string Name,
    string OperationType,
    double Minutes,
    decimal Cost
);
