using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductCardResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    string Description,
    int ImageCount
);
