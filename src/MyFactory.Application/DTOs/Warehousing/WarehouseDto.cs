using System;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.DTOs.Warehousing;

public sealed record WarehouseDto(Guid Id, string Name, string Type, string Location)
{
    public static WarehouseDto FromEntity(Warehouse warehouse)
        => new(warehouse.Id, warehouse.Name, warehouse.Type, warehouse.Location);
}
