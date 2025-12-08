using System;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.DTOs.Finance;

public sealed record RevenueReportDto(
    Guid Id,
    int PeriodMonth,
    int PeriodYear,
    Guid SpecificationId,
    string SpecificationName,
    decimal Quantity,
    decimal UnitPrice,
    decimal TotalRevenue,
    bool IsPaid,
    DateOnly? PaymentDate,
    Guid? MonthlyProfitId)
{
    public static RevenueReportDto FromEntity(RevenueReport report, string specificationName)
    {
        return new RevenueReportDto(
            report.Id,
            report.PeriodMonth,
            report.PeriodYear,
            report.SpecificationId,
            specificationName,
            report.Quantity,
            report.UnitPrice,
            report.TotalRevenue,
            report.IsPaid,
            report.PaymentDate,
            report.MonthlyProfitId);
    }
}
