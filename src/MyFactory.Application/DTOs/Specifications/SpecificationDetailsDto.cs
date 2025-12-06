using System;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationDetailsDto(
    Guid Id,
    string Sku,
    string Name,
    decimal PlanPerHour,
    string? Description,
    string Status,
    int ImagesCount)
{
    public static SpecificationDetailsDto FromEntity(Specification specification, int imagesCount = 0)
        => new(specification.Id, specification.Sku, specification.Name, specification.PlanPerHour, specification.Description, specification.Status, imagesCount);
}
