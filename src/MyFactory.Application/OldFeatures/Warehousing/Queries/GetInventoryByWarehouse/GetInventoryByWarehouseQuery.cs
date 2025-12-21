using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetInventoryByWarehouse;

public sealed record GetInventoryByWarehouseQuery(Guid WarehouseId) : IRequest<IReadOnlyCollection<InventoryItemDto>>;
