namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialResponse(
    Guid Id,
    string Code,
    string Name,
    Guid MaterialTypeId,
    string Unit,
    bool IsActive,
    decimal LastPrice,
    SupplierPrice[] Suppliers
);

public record SupplierPrice(Guid Id, string Name, decimal MaterialPrice);