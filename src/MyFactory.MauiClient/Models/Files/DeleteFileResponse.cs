namespace MyFactory.MauiClient.Models.Files;

public record DeleteFileResponse(
    FileStatus Status,
    Guid FileId);
