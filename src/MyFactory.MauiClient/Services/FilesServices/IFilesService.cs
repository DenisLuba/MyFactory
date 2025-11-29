using MyFactory.MauiClient.Models.Files;

namespace MyFactory.MauiClient.Services.FilesServices
{
    public interface IFilesService
    {
        Task<UploadFileResponse?> UploadAsync(UploadFileRequest request);
        Task<byte[]> DownloadAsync(Guid id);
        Task<DeleteFileResponse?> DeleteAsync(Guid id);
    }
}