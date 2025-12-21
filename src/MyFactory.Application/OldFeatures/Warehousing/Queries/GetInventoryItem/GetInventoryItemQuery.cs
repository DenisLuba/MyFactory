using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetInventoryItem;

public sealed record GetInventoryItemQuery(Guid WarehouseId, Guid MaterialId) : IRequest<InventoryItemDto>;
