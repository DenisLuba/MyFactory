namespace MyFactory.WebApi.Contracts.Files;

public record DeleteFileResponse(FileStatus Status, Guid FileId);

public enum FileStatus
{
    Downloaded,
    Deleted,
    Saved
}