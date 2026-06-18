using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterialIssueDetails;

public sealed class GetProductionOrderMaterialIssueDetailsQuery
    : IRequest<ProductionOrderMaterialIssueDetailsDto>
{
    public Guid ProductionOrderId { get; init; }
    public Guid MaterialId { get; init; }
}