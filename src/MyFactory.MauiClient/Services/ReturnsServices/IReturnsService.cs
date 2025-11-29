using MyFactory.MauiClient.Models.Returns;

namespace MyFactory.MauiClient.Services.ReturnsServices
{
    public interface IReturnsService
    {
        Task<ReturnsCreateResponse?> CreateReturnAsync(ReturnsCreateRequest request);
    }
}