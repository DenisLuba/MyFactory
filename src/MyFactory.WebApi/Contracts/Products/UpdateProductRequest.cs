using MyFactory.Domain.Entities.Products;

namespace MyFactory.WebApi.Contracts.Products;

public record UpdateProductRequest(
    string Name,
    Guid? ProductTypeId,
    decimal? PlanPerHour,
    ProductStatus Status,
    string? Description,
    decimal? Version);
