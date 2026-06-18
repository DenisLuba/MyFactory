using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.Products;

public record ProductListItemResponse(
    Guid Id,
    string Sku,
    string Name,
    Guid? ProductTypeId,
    string? ProductTypeName,
    ProductStatus Status,
    string? Description,
    decimal? PlanPerHour,
    decimal? Version,
    decimal CostPrice) : ListItemResponse(Id, Name);
