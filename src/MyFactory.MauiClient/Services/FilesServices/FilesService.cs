using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Files;

namespace MyFactory.MauiClient.Services.FilesServices
{
    public class FilesService(HttpClient httpClient) : IFilesService
    {
        private readonly HttpClient _httpClient = httpClient;

        /*public async Task<UploadFileResponse?> UploadAsync(UploadFileRequest request)
            => await _httpClient.PostAsJsonAsync("api/files/upload", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<UploadFileResponse>()).Unwrap();

        public async Task<byte[]> DownloadAsync(Guid id)
            => await _httpClient.GetByteArrayAsync($"api/files/{id}");

        public async Task<DeleteFileResponse?> DeleteAsync(Guid id)
            => await _httpClient.DeleteAsync($"api/files/{id}")
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<DeleteFileResponse>()).Unwrap();*/
    }
}