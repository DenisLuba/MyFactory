using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.Suppliers;

public sealed record SupplierListItemResponse(Guid Id, string Name, bool IsActive) : ListItemResponse(Id, Name);
