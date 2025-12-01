using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductBomItemResponse(
    Guid Id,
    string Material,
    double Qty,
    string Unit,
    decimal Price,
    decimal Total
);
