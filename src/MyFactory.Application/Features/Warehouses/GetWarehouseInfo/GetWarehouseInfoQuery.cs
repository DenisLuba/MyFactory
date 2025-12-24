using MediatR;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.GetWarehouseInfo;

public sealed record GetWarehouseInfoQuery(
    Guid WarehouseId
) : IRequest<WarehouseInfoDto>;