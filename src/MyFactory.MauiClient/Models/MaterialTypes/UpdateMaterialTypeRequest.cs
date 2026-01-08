namespace MyFactory.MauiClient.Models.MaterialTypes;

public sealed record UpdateMaterialTypeRequest(
    string Name,
    string? Description
);