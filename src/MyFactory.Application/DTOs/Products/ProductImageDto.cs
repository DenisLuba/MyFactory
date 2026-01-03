namespace MyFactory.Application.DTOs.Products;

public sealed record ProductImageDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string? ContentType { get; init; }
    public int SortOrder { get; init; }
    public byte[]? Content { get; init; }
}
