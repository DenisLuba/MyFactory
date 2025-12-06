using System;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.Materials;

public sealed record MaterialDto(Guid Id, string Name, string Unit, bool IsActive, MaterialTypeDto MaterialType)
{
    public static MaterialDto FromEntity(Material material, MaterialTypeDto materialType)
        => new(material.Id, material.Name, material.Unit, material.IsActive, materialType);
}
