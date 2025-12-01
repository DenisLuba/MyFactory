using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductBomDeleteResponse(
    Guid LineId,
    string Status
);
