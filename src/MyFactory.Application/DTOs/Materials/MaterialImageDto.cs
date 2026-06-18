namespace MyFactory.Application.DTOs.Materials;

public sealed record MaterialImageDto
{
    public Guid Id { get; init; }
    public Guid MaterialId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string? ContentType { get; init; }
    public int SortOrder { get; init; }
    public byte[]? Content { get; init; }
}
