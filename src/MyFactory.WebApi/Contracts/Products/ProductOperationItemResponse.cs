using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductOperationItemResponse(
    Guid Id,
    string Operation,
    double Minutes,
    decimal Cost
);
