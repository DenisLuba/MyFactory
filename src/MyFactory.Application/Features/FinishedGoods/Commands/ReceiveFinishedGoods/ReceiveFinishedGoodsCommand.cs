using System;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Commands.ReceiveFinishedGoods;

public sealed record ReceiveFinishedGoodsCommand(
    Guid SpecificationId,
    Guid WarehouseId,
    decimal Quantity,
    decimal UnitCost,
    DateTime ReceivedAt) : IRequest<FinishedGoodsInventoryDto>;
