using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.ProductionCostFacts;

public sealed record CalculateProductionCostFactCommand(
    int PeriodMonth,
    int PeriodYear,
    Guid SpecificationId,
    decimal QuantityProduced,
    decimal MaterialCost,
    decimal LaborCost) : IRequest<ProductionCostFactDto>;
