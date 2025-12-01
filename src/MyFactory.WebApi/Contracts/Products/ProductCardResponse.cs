using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductCardResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    string Description,
    int ImageCount
);
