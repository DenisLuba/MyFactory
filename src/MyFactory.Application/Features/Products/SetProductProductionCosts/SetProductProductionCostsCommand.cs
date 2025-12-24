using MediatR;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.SetProductProductionCosts;

public sealed record SetProductProductionCostsCommand(
    Guid ProductId,
    IReadOnlyCollection<ProductDepartmentCostDto> Costs
) : IRequest;
