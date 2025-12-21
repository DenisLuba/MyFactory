using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.OldFeatures.Production.Commands.RecordProductionStage;

public sealed record RecordProductionStageCommand(
    Guid ProductionOrderId,
    Guid WorkshopId,
    string StageType,
    decimal QtyIn,
    decimal QtyOut,
    DateTime RecordedAt) : IRequest<ProductionOrderDto>;
