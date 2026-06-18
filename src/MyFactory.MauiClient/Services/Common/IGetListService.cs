using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Services.Common;

public interface IGetListService<T> where T : ListItemResponse
{
    Task<ListResponse<T>?> GetListAsync(
        string? searchName = null,
        string? searchType = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = null,
        Guid? fromId = null);
}


