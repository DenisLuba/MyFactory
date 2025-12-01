using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductsListResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    string Status,
    int ImageCount
);
