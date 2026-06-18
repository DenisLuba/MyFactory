namespace MyFactory.MauiClient.Models.ProductTypes;

public sealed record CreateProductTypeRequest(
    string Type,
    string? Description
);
