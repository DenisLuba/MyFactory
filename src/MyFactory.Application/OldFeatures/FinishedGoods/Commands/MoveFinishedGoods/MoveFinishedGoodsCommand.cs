using System;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Commands.MoveFinishedGoods;

public sealed record MoveFinishedGoodsCommand(
    Guid SpecificationId,
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    decimal Quantity,
    DateTime MovedAt) : IRequest<FinishedGoodsMovementDto>;
