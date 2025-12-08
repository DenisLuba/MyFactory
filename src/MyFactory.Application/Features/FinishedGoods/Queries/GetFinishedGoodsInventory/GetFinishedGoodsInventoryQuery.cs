using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Queries.GetFinishedGoodsInventory;

public sealed record GetFinishedGoodsInventoryQuery(Guid? SpecificationId, Guid? WarehouseId)
    : IRequest<IReadOnlyCollection<FinishedGoodsInventoryDto>>;
