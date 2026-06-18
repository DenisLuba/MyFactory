namespace MyFactory.WebApi.Contracts.Suppliers;

public sealed record SupplierListItemResponse(Guid Id, string Name, bool IsActive);
