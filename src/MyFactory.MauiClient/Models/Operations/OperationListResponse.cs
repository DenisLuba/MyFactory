using System;

namespace MyFactory.MauiClient.Models.Operations;

public record OperationListResponse(
    Guid Id,
    string Code,
    string Name,
    string OperationType,
    double Minutes,
    decimal Cost
);
