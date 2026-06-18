namespace MyFactory.WebApi.Contracts.ProductTypes;

public sealed record CreateProductTypeRequest(string Type, string? Description);
