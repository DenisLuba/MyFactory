using System;

namespace MyFactory.MauiClient.Models.Operations;

public record OperationUpdateResponse(
    Guid Id,
    string Status
);
