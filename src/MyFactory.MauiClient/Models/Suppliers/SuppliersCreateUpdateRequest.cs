namespace MyFactory.MauiClient.Models.Suppliers;

public record SuppliersCreateUpdateRequest(
    string Name,
    SupplierTypes SupplierType,
    SupplierStatus Status,
    string? Address,
    string? Phone,
    string? Email
);
