using System.Diagnostics;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Specifications;

namespace MyFactory.MauiClient.Services.SpecificationsServices
{
    public class SpecificationsService(HttpClient httpClient) : ISpecificationsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<SpecificationsGetResponse>?> ListAsync()
            => await _httpClient.GetFromJsonAsync<List<SpecificationsGetResponse>>("api/specifications");

        public async Task<SpecificationsGetResponse?> GetAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<SpecificationsGetResponse>($"api/specifications/{id}");

        public async Task<SpecificationsCreateResponse?> CreateAsync(SpecificationsCreateRequest request)
            => await _httpClient.PostAsJsonAsync("api/specifications", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SpecificationsCreateResponse>()).Unwrap();

        public async Task<SpecificationsUpdateResponse?> UpdateAsync(Guid id, SpecificationsUpdateRequest request)
            => await _httpClient.PutAsJsonAsync($"api/specifications/{id}", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SpecificationsUpdateResponse>()).Unwrap();

        public async Task<SpecificationsAddBomResponse?> AddBomAsync(Guid id, SpecificationsAddBomRequest request)
            => await _httpClient.PostAsJsonAsync($"api/specifications/{id}/bom", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SpecificationsAddBomResponse>()).Unwrap();

        public async Task<SpecificationsDeleteBomItemResponse?> DeleteBomItemAsync(Guid id, Guid bomId)
            => await _httpClient.DeleteAsync($"api/specifications/{id}/bom/{bomId}")
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SpecificationsDeleteBomItemResponse>()).Unwrap();

        public async Task<SpecificationsAddOperationResponse?> AddOperationAsync(Guid id, SpecificationsAddOperationRequest request)
            => await _httpClient.PostAsJsonAsync($"api/specifications/{id}/operations", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SpecificationsAddOperationResponse>()).Unwrap();

        public async Task<SpecificationsUploadImageResponse?> UploadImageAsync(Guid id, Stream imageStream, string fileName, string? contentType = "image/jpeg")
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(imageStream);
                if (!string.IsNullOrEmpty(contentType))
                {
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                }
                content.Add(streamContent, "file", fileName);

                var response = await _httpClient.PostAsync($"api/specifications/{id}/images", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SpecificationsUploadImageResponse>();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }

        public async Task<SpecificationsCostResponse?> CostAsync(Guid id, DateTime? asOf = null)
            => await _httpClient.GetFromJsonAsync<SpecificationsCostResponse>($"api/specifications/{id}/cost{(asOf.HasValue ? "?asOf=" + asOf.Value.ToString("yyyy-MM-dd") : "")}");
    }
}