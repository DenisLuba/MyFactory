namespace MyFactory.MauiClient.Models.Suppliers;

public record SupplierResponse(
    Guid Id,
    string Name,
    SupplierTypes SupplierType,
    SupplierStatus Status,
    string? Address,
    string? Phone,
    string? Email
);


