namespace MyFactory.WebApi.Contracts.Operations;

public record OperationUpdateRequest(
    string Code,
    string Name,
    string OperationType,
    double Minutes,
    decimal Cost
);
