namespace MyFactory.MauiClient.Models.ProductTypes;

public sealed record UpdateProductTypeRequest(
    string Type,
    string? Description
);
