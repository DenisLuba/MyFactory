namespace MyFactory.MauiClient.Models.Materials;

public record MaterialsListResponse(
    IReadOnlyList<MaterialListItemResponse> Items,
    int TotalCount,
    int Take,
    int Skip,
    bool HasMore);
