using System;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.Materials;

public sealed record SupplierDto(Guid Id, string Name, string Contact)
{
    public static SupplierDto FromEntity(Supplier supplier) => new(supplier.Id, supplier.Name, supplier.Contact);
}
