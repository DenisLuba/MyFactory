namespace MyFactory.WebApi.Contracts.Suppliers;

public record SuppliersCreateUpdateRequest(
    string Name,
    SupplierTypes SupplierType,
    SupplierStatus Status,
    string? Address,
    string? Phone,
    string? Email
);
