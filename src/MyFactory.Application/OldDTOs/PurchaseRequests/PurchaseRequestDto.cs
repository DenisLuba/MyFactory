using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldDTOs.PurchaseRequests;

public sealed record PurchaseRequestDto(
    Guid Id,
    string PrNumber,
    DateTime CreatedAt,
    string Status,
    IReadOnlyCollection<PurchaseRequestItemDto> Items)
{
    public static PurchaseRequestDto FromEntity(PurchaseRequest request, IReadOnlyCollection<PurchaseRequestItemDto> items)
        => new(request.Id, request.PrNumber, request.CreatedAt, request.Status, items);

    public static PurchaseRequestDto FromEntity(PurchaseRequest request, IReadOnlyCollection<PurchaseRequestItem> items, IReadOnlyDictionary<Guid, Material> materials)
    {
        var itemDtos = items
            .Select(item => PurchaseRequestItemDto.FromEntity(item, materials[item.MaterialId]))
            .ToList();

        return FromEntity(request, itemDtos);
    }
}
