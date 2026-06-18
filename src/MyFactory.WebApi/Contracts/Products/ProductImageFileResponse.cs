namespace MyFactory.WebApi.Contracts.Products;

public record ProductImageFileResponse(
    Guid Id,
    Guid ProductId,
    string FileName,
    string? ContentType,
    byte[]? Content);
