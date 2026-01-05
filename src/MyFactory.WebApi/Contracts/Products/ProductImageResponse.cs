namespace MyFactory.WebApi.Contracts.Products;

public record ProductImageResponse(
    Guid Id,
    Guid ProductId,
    string FileName,
    string Path,
    string? ContentType,
    int SortOrder);
