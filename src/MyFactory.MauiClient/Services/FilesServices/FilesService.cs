using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Files;

namespace MyFactory.MauiClient.Services.FilesServices
{
    public class FilesService(HttpClient httpClient) : IFilesService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<UploadFileResponse?> UploadAsync(UploadFileRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var formData = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(request.FileBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
            formData.Add(fileContent, "File", request.FileName);

            using var response = await _httpClient.PostAsync("api/files/upload", formData);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UploadFileResponse>();
        }

        public async Task<byte[]> DownloadAsync(Guid id)
            => await _httpClient.GetByteArrayAsync($"api/files/{id}");

        public async Task<DeleteFileResponse?> DeleteAsync(Guid id)
            => await _httpClient.DeleteAsync($"api/files/{id}")
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<DeleteFileResponse>()).Unwrap();
    }
}