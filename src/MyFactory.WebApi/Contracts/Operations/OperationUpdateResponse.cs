using System;

namespace MyFactory.WebApi.Contracts.Operations;

public record OperationUpdateResponse(
    Guid Id,
    string Status
);
