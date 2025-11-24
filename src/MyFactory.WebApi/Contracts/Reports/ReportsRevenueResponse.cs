namespace MyFactory.WebApi.Contracts.Reports;

public record ReportsRevenueResponse(
    Guid SpecificationId,
    string SpecificationName,
    decimal Revenue
);

