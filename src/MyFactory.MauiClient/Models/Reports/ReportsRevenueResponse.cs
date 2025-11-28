namespace MyFactory.MauiClient.Models.Reports;

public record ReportsRevenueResponse(
    Guid SpecificationId,
    string SpecificationName,
    decimal Revenue
);

