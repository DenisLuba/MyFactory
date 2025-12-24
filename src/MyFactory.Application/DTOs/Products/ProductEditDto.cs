using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.DTOs.Products;

public sealed class ProductEditDto
{
    public Guid? Id { get; init; } // null → create
    public string Sku { get; init; } = null!;
    public string Name { get; init; } = null!;
    public decimal? PlanPerHour { get; init; }
    public ProductStatus Status { get; init; }
}