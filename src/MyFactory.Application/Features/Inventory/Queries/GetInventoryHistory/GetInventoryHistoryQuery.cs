using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.Features.Inventory.Queries.GetInventoryHistory;

public sealed record GetInventoryHistoryQuery(
    Guid MaterialId,
    Guid? WarehouseId,
    DateOnly? FromDate,
    DateOnly? ToDate) : IRequest<IReadOnlyCollection<InventoryHistoryEntryDto>>;
