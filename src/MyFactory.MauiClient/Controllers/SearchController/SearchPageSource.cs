using Microsoft.IdentityModel.Abstractions;
using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Services.Common;
using MyFactory.MauiClient.Services.Materials;

namespace MyFactory.MauiClient.Controllers;

public sealed class SearchPageSource<T>(
    IGetListService<T> service,
    string? sortBy = null,
    bool sortDesk = false,
    bool? isActive = null,
    Guid? fromId = null
) : ISearchPageSouce where T : ListItemResponse
{
    public async Task<ListResponse<SearchItemDto>?> GetPageAsync(
        int skip, 
        int take,  
        string? searchName, 
        string? searchType,
        CancellationToken cs = default)
    {
        var response = await service.GetListAsync(
            searchName: searchName,
            searchType: searchType,
            sortBy: sortBy,
            sortDesc: sortDesk,
            skip: skip,
            take: take,
            isActive: isActive,
            fromId: fromId);

        if (response is null) return null;

        var items = response.Items
            .Select(i => new SearchItemDto(i.Id.ToString(), i.Name))
            .Where(i => !string.IsNullOrWhiteSpace(i.Id) && !string.IsNullOrWhiteSpace(i.Name))
            .ToList();

        return new ListResponse<SearchItemDto>(
            Items: items,
            TotalCount: response.TotalCount,
            Take: response.Take,
            Skip: response.Skip,
            HasMore: response.HasMore
        );
    }
}
