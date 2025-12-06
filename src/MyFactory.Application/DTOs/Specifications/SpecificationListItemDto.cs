using System;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationListItemDto(
    Guid Id,
    string Sku,
    string Name,
    decimal PlanPerHour,
    string Status,
    int ImagesCount)
{
    public static SpecificationListItemDto FromEntity(Specification specification, int imagesCount = 0)
        => new(specification.Id, specification.Sku, specification.Name, specification.PlanPerHour, specification.Status, imagesCount);
}
