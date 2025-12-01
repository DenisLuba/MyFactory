using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductOperationItemResponse(
    Guid Id,
    string Operation,
    double Minutes,
    decimal Cost
);
