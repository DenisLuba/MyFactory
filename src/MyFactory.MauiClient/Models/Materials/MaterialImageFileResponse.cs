namespace MyFactory.MauiClient.Models.Materials;

public record MaterialImageFileResponse(
    Guid Id,
    Guid MaterialId,
    string FileName,
    string? ContentType,
    byte[]? Content);