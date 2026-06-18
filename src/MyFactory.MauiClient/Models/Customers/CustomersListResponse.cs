namespace MyFactory.MauiClient.Models.Customers;

public record CustomersListResponse(
    IReadOnlyList<CustomerListItemResponse> Items,
    int TotalCount,
    int Take,
    int Skip,
    bool HasMore);
