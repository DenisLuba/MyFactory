namespace MyFactory.MauiClient.Models.Materials;

public record MaterialResponse(
    Guid Id,
    string Code,
    string Name,
    Guid MaterialTypeId,
    string Unit,
    bool IsActive,
    decimal LastPrice,
    SupplierPrice[] Suppliers);
