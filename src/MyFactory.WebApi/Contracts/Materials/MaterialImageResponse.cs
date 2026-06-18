namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialImageResponse(
    Guid Id,
    Guid MaterialId,
    string FileName,
    string Path,
    string? ContentType,
    int SortOrder);
