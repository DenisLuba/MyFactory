namespace MyFactory.WebApi.Contracts.Common;

public record ListResponse<TItem>(
    IReadOnlyList<TItem> Items,
    int TotalCount,
    int Take,
    int Skip,
    bool HasMore);