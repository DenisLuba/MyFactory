using System;

namespace MyFactory.WebApi.Contracts.Operations;

public record OperationCardResponse(
    Guid Id,
    string Code,
    string Name,
    string OperationType,
    double Minutes,
    decimal Cost
);
