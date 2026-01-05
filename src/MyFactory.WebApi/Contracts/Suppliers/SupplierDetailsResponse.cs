namespace MyFactory.WebApi.Contracts.Suppliers;

public sealed record SupplierDetailsResponse(
    Guid Id,
    string Name,
    string? Description,
    IReadOnlyList<SupplierPurchaseHistoryResponse> Purchases);
