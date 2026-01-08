namespace MyFactory.MauiClient.Models.MaterialTypes;

public sealed record CreateMaterialTypeRequest(
    string Name,
    string? Description
);
