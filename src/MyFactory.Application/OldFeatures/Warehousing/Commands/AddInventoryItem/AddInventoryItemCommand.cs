using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.AddInventoryItem;

public sealed record AddInventoryItemCommand(Guid WarehouseId, Guid MaterialId) : IRequest<InventoryItemDto>;
