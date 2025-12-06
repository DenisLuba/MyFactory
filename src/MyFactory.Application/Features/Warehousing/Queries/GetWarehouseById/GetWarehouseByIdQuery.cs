using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetWarehouseById;

public sealed record GetWarehouseByIdQuery(Guid Id) : IRequest<WarehouseDto>;
