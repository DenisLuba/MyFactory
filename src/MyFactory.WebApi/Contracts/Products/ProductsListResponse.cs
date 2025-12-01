using System;

namespace MyFactory.WebApi.Contracts.Products;

public record ProductsListResponse(
    Guid Id,
    string Sku,
    string Name,
    double PlanPerHour,
    string Status,
    int ImageCount
);
