namespace MyFactory.MauiClient.Models.MaterialTypes;

public sealed record MaterialTypeModel(
    Guid Id,
    string Name,
    string? Description
);