namespace MyFactory.WebApi.Contracts.ProductTypes;

public sealed record ProductTypeResponse(Guid Id, string Type, string? Description);
