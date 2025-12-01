using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Returns;

namespace MyFactory.MauiClient.Services.ReturnsServices
{
    public interface IReturnsService
    {
        Task<IReadOnlyList<ReturnsListResponse>> GetReturnsAsync(CancellationToken cancellationToken = default);
        Task<ReturnCardResponse?> GetReturnAsync(Guid returnId, CancellationToken cancellationToken = default);
        Task<ReturnsCreateResponse?> CreateReturnAsync(ReturnsCreateRequest request, CancellationToken cancellationToken = default);
    }
}