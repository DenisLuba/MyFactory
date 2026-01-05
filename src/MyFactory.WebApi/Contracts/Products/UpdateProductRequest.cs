using MyFactory.Domain.Entities.Products;

namespace MyFactory.WebApi.Contracts.Products;

public record UpdateProductRequest(
    string Name,
    int? PlanPerHour,
    ProductStatus Status);
