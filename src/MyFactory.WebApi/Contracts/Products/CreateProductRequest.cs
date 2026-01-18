using MyFactory.Domain.Entities.Products;

namespace MyFactory.WebApi.Contracts.Products;

public record CreateProductRequest(
    string Sku,
    string Name,
    ProductStatus Status,
    int? PlanPerHour,
    string? Description,
    int? Version);
