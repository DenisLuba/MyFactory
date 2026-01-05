namespace MyFactory.MauiClient.Models.Products;

public record ProductImageFileResponse(
    Guid Id,
    Guid ProductId,
    string FileName,
    string? ContentType,
    byte[]? Content);
