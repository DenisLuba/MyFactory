using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductOperationDeleteResponse(
    Guid LineId,
    string Status
);
