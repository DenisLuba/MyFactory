using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductBomDeleteResponse(
    Guid LineId,
    string Status
);
