using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductUpdateResponse(
    Guid Id,
    string Status
);
