using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductOperationDeleteResponse(
    Guid LineId,
    string Status
);
