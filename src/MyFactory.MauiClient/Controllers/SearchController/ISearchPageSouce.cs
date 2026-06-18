using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Controllers;

public interface ISearchPageSouce
{
    Task<ListResponse<SearchItemDto>?> GetPageAsync(
        int skip,
        int take,
        string? searchName = null,
        string? searchType = null,
        CancellationToken ct = default);
}
