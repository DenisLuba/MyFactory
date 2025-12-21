using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetWarehouseById;

public sealed record GetWarehouseByIdQuery(Guid Id) : IRequest<WarehouseDto>;
