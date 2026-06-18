namespace MyFactory.MauiClient.Models.ProductTypes;

public sealed record ProductTypeModel(
    Guid Id,
    string Type,
    string? Description
);
