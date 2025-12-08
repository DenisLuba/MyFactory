using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.Features.Inventory.Queries.GetInventoryHistory;

public sealed record GetInventoryHistoryQuery(
    Guid MaterialId,
    Guid? WarehouseId,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<IReadOnlyCollection<InventoryHistoryEntryDto>>;
