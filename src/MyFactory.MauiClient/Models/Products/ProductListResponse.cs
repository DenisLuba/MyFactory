namespace MyFactory.MauiClient.Models.Products;

public record ProductListResponse(
	IReadOnlyList<ProductListItemResponse> Items,
	int TotalCount,
	int Take,
	int Skip,
	bool HasMore);