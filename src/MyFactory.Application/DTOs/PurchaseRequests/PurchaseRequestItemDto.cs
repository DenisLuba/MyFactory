using System;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.DTOs.PurchaseRequests;

public sealed record PurchaseRequestItemDto(Guid Id, Guid MaterialId, string MaterialName, decimal Quantity)
{
    public static PurchaseRequestItemDto FromEntity(PurchaseRequestItem item, Material material)
        => new(item.Id, item.MaterialId, material.Name, item.Quantity);
}
