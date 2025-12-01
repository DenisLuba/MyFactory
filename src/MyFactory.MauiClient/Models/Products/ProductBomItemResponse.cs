using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductBomItemResponse(
    Guid Id,
    string Material,
    double Qty,
    string Unit,
    decimal Price,
    decimal Total
);
