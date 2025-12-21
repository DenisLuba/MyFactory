using System;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.OldDTOs.Materials;

public sealed record MaterialPriceHistoryDto(
    Guid Id,
    Guid MaterialId,
    decimal Price,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    string DocRef,
    SupplierDto Supplier)
{
    public static MaterialPriceHistoryDto FromEntity(MaterialPriceHistory entry, SupplierDto supplier)
        => new(entry.Id, entry.MaterialId, entry.Price, entry.EffectiveFrom, entry.EffectiveTo, entry.DocRef, supplier);
}
