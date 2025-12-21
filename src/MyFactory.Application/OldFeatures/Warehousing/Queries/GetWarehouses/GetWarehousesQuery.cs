using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetWarehouses;

public sealed record GetWarehousesQuery : IRequest<IReadOnlyCollection<WarehouseDto>>;
