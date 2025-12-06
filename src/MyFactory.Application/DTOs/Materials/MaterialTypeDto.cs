using System;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.Materials;

public sealed record MaterialTypeDto(Guid Id, string Name)
{
    public static MaterialTypeDto FromEntity(MaterialType type) => new(type.Id, type.Name);
}
