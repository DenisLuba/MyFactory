using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStages;

public sealed record GetProductionStagesQuery(Guid ProductionOrderId) : IRequest<IReadOnlyList<ProductionStageSummaryDto>>;
